using HelseFlow_Backend.Application.DTOs;
using HelseFlow_Backend.Application.Interfaces;
using HelseFlow_Backend.Domain.Entities;

namespace HelseFlow_Backend.Application.Services;

public class PatientService
{
    private readonly IVitalLogRepository _vitalLogRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public PatientService(IVitalLogRepository vitalLogRepository, IUserRepository userRepository, IDoctorRepository doctorRepository, IAppointmentRepository appointmentRepository)
    {
        _vitalLogRepository = vitalLogRepository;
        _userRepository = userRepository;
        _doctorRepository = doctorRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<VitalLogDto> LogVitalsAsync(Guid patientId, DateTime date, int bloodPressureSystolic, int bloodPressureDiastolic, int pulse, double weight, string mood)
    {
        var vitalLog = new VitalLog(
            Guid.NewGuid(),
            patientId,
            date,
            bloodPressureSystolic,
            bloodPressureDiastolic,
            pulse,
            weight,
            mood
        );

        await _vitalLogRepository.AddAsync(vitalLog);

        return new VitalLogDto
        {
            Id = vitalLog.Id,
            PatientId = vitalLog.PatientId,
            Date = vitalLog.Date,
            BloodPressureSystolic = vitalLog.BloodPressureSystolic,
            BloodPressureDiastolic = vitalLog.BloodPressureDiastolic,
            Pulse = vitalLog.Pulse,
            Weight = vitalLog.Weight,
            Mood = vitalLog.Mood
        };
    }

    public async Task<List<VitalLogDto>> GetPatientVitalLogsAsync(Guid patientId, DateTime? startDate, DateTime? endDate)
    {
        var vitalLogs = await _vitalLogRepository.GetByPatientIdAsync(patientId, startDate, endDate);
        return vitalLogs.Select(vl => new VitalLogDto
        {
            Id = vl.Id,
            PatientId = vl.PatientId,
            Date = vl.Date,
            BloodPressureSystolic = vl.BloodPressureSystolic,
            BloodPressureDiastolic = vl.BloodPressureDiastolic,
            Pulse = vl.Pulse,
            Weight = vl.Weight,
            Mood = vl.Mood
        }).ToList();
    }

    public async Task<List<DoctorDto>> GetAllDoctorsAsync(string? specialty, string? search)
    {
        var doctors = await _doctorRepository.GetAllAsync(specialty, search);
        return doctors.Select(d => new DoctorDto
        {
            Id = d.Id,
            Name = d.Name,
            Specialty = d.Specialty,
            Location = d.Location,
            ImageUrl = d.ImageUrl
        }).ToList();
    }

    public async Task<DoctorDto?> GetDoctorByIdAsync(Guid doctorId)
    {
        var doctor = await _doctorRepository.GetByIdAsync(doctorId);
        if (doctor == null) return null;

        return new DoctorDto
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Specialty = doctor.Specialty,
            Location = doctor.Location,
            ImageUrl = doctor.ImageUrl,
            Bio = doctor.Bio
        };
    }

    public async Task<AppointmentDto?> BookAppointmentAsync(Guid patientId, Guid doctorId, DateTime appointmentTime, string reason)
    {
        // Check slot availability
        var isAvailable = await _appointmentRepository.IsAppointmentSlotAvailableAsync(doctorId, appointmentTime);
        if (!isAvailable)
        {
            return null; // Slot unavailable
        }

        var doctor = await _doctorRepository.GetByIdAsync(doctorId);
        if (doctor == null)
        {
            return null; // Doctor not found
        }

        var appointment = new Appointment(
            Guid.NewGuid(),
            patientId,
            doctorId,
            appointmentTime,
            reason,
            "Confirmed" // Or "Pending" depending on business logic
        );

        await _appointmentRepository.AddAsync(appointment);

        return new AppointmentDto
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            DoctorId = appointment.DoctorId,
            DoctorName = doctor.Name,
            AppointmentTime = appointment.AppointmentTime,
            Reason = appointment.Reason,
            Status = appointment.Status
        };
    }

    public async Task<List<AppointmentDto>> GetPatientAppointmentsAsync(Guid patientId, string? status)
    {
        var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId, status);
        var doctorIds = appointments.Select(a => a.DoctorId).Distinct().ToList();
        var doctors = (await Task.WhenAll(doctorIds.Select(id => _doctorRepository.GetByIdAsync(id))))
            .Where(d => d != null)
            .ToDictionary(d => d!.Id, d => d!.Name);

        return appointments.Select(a => new AppointmentDto
        {
            Id = a.Id,
            PatientId = a.PatientId,
            DoctorId = a.DoctorId,
            DoctorName = doctors.GetValueOrDefault(a.DoctorId, "Unknown Doctor"),
            AppointmentTime = a.AppointmentTime,
            Reason = a.Reason,
            Status = a.Status
        }).ToList();
    }
}
