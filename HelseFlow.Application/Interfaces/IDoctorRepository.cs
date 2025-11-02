using HelseFlow_Backend.Domain.Entities;

namespace HelseFlow_Backend.Application.Interfaces;

public interface IDoctorRepository
{
    Task<List<Doctor>> GetAllAsync(string? specialty, string? search);
    Task<Doctor?> GetByIdAsync(Guid id);
}
