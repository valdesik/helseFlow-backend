using HelseFlow_Backend.Application.Interfaces;
using HelseFlow_Backend.Domain.Entities;

namespace HelseFlow_Backend.Infrastructure.Data;

public class InMemoryAppointmentRepository : IAppointmentRepository
{
    private readonly List<Appointment> _appointments = new();

    public Task AddAsync(Appointment appointment)
    {
        _appointments.Add(appointment);
        return Task.CompletedTask;
    }

    public Task<List<Appointment>> GetByPatientIdAsync(Guid patientId, string? status)
    {
        var query = _appointments.Where(a => a.PatientId == patientId);

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(query.ToList());
    }

    public Task<bool> IsAppointmentSlotAvailableAsync(Guid doctorId, DateTime appointmentTime)
    {
        // For simplicity, assume a slot is taken if an appointment already exists for that doctor at that time
        var isBooked = _appointments.Any(
            a => a.DoctorId == doctorId &&
                 a.AppointmentTime == appointmentTime &&
                 a.Status != "Cancelled"
        );
        return Task.FromResult(!isBooked);
    }
}
