using HelseFlow_Backend.Application.DTOs;
using HelseFlow_Backend.Application.Interfaces;
using HelseFlow_Backend.Domain.Entities;

namespace HelseFlow_Backend.Application.Services;

public class AdminService
{
    private readonly IUserRepository _userRepository;
    private readonly IVitalLogRepository _vitalLogRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IDoctorRepository _doctorRepository;

    public AdminService(IUserRepository userRepository, IVitalLogRepository vitalLogRepository, IAppointmentRepository appointmentRepository, IDoctorRepository doctorRepository)
    {
        _userRepository = userRepository;
        _vitalLogRepository = vitalLogRepository;
        _appointmentRepository = appointmentRepository;
        _doctorRepository = doctorRepository;
    }

    public async Task<List<UserDto>> GetAllPatientsAsync(string? search, string? region, int? ageMin, int? ageMax)
    {
        var patients = await _userRepository.GetAllPatientsAsync(search, region, ageMin, ageMax);
        return patients.Select(p => new UserDto
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email,
            Role = p.Role,
            Age = p.Age,
            Region = p.Region,
            RiskFactors = p.RiskFactors
        }).ToList();
    }

    public async Task<PatientDetailsDto?> GetPatientDetailsAsync(Guid patientId)
    {
        var patient = await _userRepository.GetByIdAsync(patientId);
        if (patient == null || patient.Role != "patient")
        {
            return null;
        }

        var vitals = await _vitalLogRepository.GetByPatientIdAsync(patientId, null, null);
        var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId, null);

        var doctorIds = appointments.Select(a => a.DoctorId).Distinct().ToList();
        var doctors = (await Task.WhenAll(doctorIds.Select(id => _doctorRepository.GetByIdAsync(id))))
            .Where(d => d != null)
            .ToDictionary(d => d!.Id, d => d!.Name);

        return new PatientDetailsDto
        {
            Id = patient.Id,
            Name = patient.Name,
            Email = patient.Email,
            Age = patient.Age,
            Region = patient.Region,
            RiskFactors = patient.RiskFactors,
            Vitals = vitals.Select(vl => new VitalLogDto
            {
                Id = vl.Id,
                PatientId = vl.PatientId,
                Date = vl.Date,
                BloodPressureSystolic = vl.BloodPressureSystolic,
                BloodPressureDiastolic = vl.BloodPressureDiastolic,
                Pulse = vl.Pulse,
                Weight = vl.Weight,
                Mood = vl.Mood
            }).ToList(),
            Appointments = appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                PatientId = a.PatientId,
                DoctorId = a.DoctorId,
                DoctorName = doctors.GetValueOrDefault(a.DoctorId, "Unknown Doctor"),
                AppointmentTime = a.AppointmentTime,
                Reason = a.Reason,
                Status = a.Status
            }).ToList()
        };
    }

    public async Task<UserDto?> UpdatePatientProfileAsync(Guid patientId, string name, string email, int? age, string? region, List<string>? riskFactors)
    {
        var patient = await _userRepository.GetByIdAsync(patientId);
        if (patient == null || patient.Role != "patient")
        {
            return null; // Patient not found or not a patient role
        }

        // Check if the new email is already taken by another user
        if (!string.Equals(patient.Email, email, StringComparison.OrdinalIgnoreCase))
        {
            var existingUserWithNewEmail = await _userRepository.GetByEmailAsync(email);
            if (existingUserWithNewEmail != null)
            {
                return null; // Email already taken
            }
        }

        // Create a new User object with updated data, as User is immutable
        var updatedPatient = new User(
            patient.Id,
            name,
            email,
            patient.PasswordHash,
            patient.Role,
            age,
            region,
            riskFactors
        );

        await _userRepository.UpdateAsync(updatedPatient);

        return new UserDto
        {
            Id = updatedPatient.Id,
            Name = updatedPatient.Name,
            Email = updatedPatient.Email,
            Role = updatedPatient.Role,
            Age = updatedPatient.Age,
            Region = updatedPatient.Region,
            RiskFactors = updatedPatient.RiskFactors
        };
    }

    public async Task<bool> DeletePatientAsync(Guid patientId)
    {
        var patient = await _userRepository.GetByIdAsync(patientId);
        if (patient == null || patient.Role != "patient")
        {
            return false; // Patient not found or not a patient role
        }

        await _userRepository.DeleteAsync(patientId);
        // In a real application, all associated data (vitals, appointments, etc.) should also be deleted here
        return true;
    }

    public async Task<List<RegionalHealthStatsDto>> GetRegionalHealthStatisticsAsync(string? region, string? ageGroup, string? gender)
    {
        // For simplicity, this implementation will use data from InMemoryUserRepository and InMemoryVitalLogRepository.
        // In a real application, this would involve aggregating data, possibly from external APIs.

        var allPatients = await _userRepository.GetAllPatientsAsync(null, region, null, null);
        var allVitalLogs = new List<VitalLog>();

        foreach (var patient in allPatients)
        {
            allVitalLogs.AddRange(await _vitalLogRepository.GetByPatientIdAsync(patient.Id, null, null));
        }

        var stats = allPatients
            .GroupBy(p => p.Region ?? "Unknown")
            .Select(g =>
            {
                var patientsInRegion = g.ToList();
                var vitalLogsInRegion = allVitalLogs.Where(vl => patientsInRegion.Any(p => p.Id == vl.PatientId)).ToList();

                var avgSystolic = vitalLogsInRegion.Any() ? vitalLogsInRegion.Average(vl => vl.BloodPressureSystolic) : 0;
                var avgDiastolic = vitalLogsInRegion.Any() ? vitalLogsInRegion.Average(vl => vl.BloodPressureDiastolic) : 0;

                // Example of defining patients at risk of hypertension (simplified)
                var patientsAtRiskHypertension = vitalLogsInRegion.Count(vl => vl.BloodPressureSystolic > 130 || vl.BloodPressureDiastolic > 85);

                return new RegionalHealthStatsDto
                {
                    Region = g.Key,
                    AvgBloodPressureSystolic = Math.Round(avgSystolic, 2),
                    AvgBloodPressureDiastolic = Math.Round(avgDiastolic, 2),
                    PatientsAtRiskHypertension = patientsAtRiskHypertension,
                    TotalPatients = patientsInRegion.Count
                };
            })
            .ToList();

        return stats;
    }
}

public class PatientDetailsDto : UserDto
{
    public List<VitalLogDto> Vitals { get; set; } = new List<VitalLogDto>();
    public List<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();
}
