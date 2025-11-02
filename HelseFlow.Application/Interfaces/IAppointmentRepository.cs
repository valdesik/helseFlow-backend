using HelseFlow_Backend.Domain.Entities;

namespace HelseFlow_Backend.Application.Interfaces;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment appointment);
    Task<List<Appointment>> GetByPatientIdAsync(Guid patientId, string? status);
    Task<bool> IsAppointmentSlotAvailableAsync(Guid doctorId, DateTime appointmentTime);
}
