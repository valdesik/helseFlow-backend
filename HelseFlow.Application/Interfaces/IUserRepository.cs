using HelseFlow_Backend.Domain.Entities;

namespace HelseFlow_Backend.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
    Task<User?> GetByIdAsync(Guid id);
    Task<List<User>> GetAllPatientsAsync(string? search, string? region, int? ageMin, int? ageMax);
}
