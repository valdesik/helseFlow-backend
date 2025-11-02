using HelseFlow_Backend.Domain.Entities;

namespace HelseFlow_Backend.Application.Interfaces;

public interface IVitalLogRepository
{
    Task AddAsync(VitalLog vitalLog);
    Task<List<VitalLog>> GetByPatientIdAsync(Guid patientId, DateTime? startDate, DateTime? endDate);
}
