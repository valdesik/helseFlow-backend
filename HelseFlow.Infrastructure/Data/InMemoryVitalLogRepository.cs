using HelseFlow_Backend.Application.Interfaces;
using HelseFlow_Backend.Domain.Entities;

namespace HelseFlow_Backend.Infrastructure.Data;

public class InMemoryVitalLogRepository : IVitalLogRepository
{
    private readonly List<VitalLog> _vitalLogs = new();

    public Task AddAsync(VitalLog vitalLog)
    {
        _vitalLogs.Add(vitalLog);
        return Task.CompletedTask;
    }

    public Task<List<VitalLog>> GetByPatientIdAsync(Guid patientId, DateTime? startDate, DateTime? endDate)
    {
        var query = _vitalLogs.Where(vl => vl.PatientId == patientId);

        if (startDate.HasValue)
        {
            query = query.Where(vl => vl.Date >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(vl => vl.Date <= endDate.Value);
        }

        return Task.FromResult(query.ToList());
    }
}
