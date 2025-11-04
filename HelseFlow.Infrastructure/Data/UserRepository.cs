using HelseFlow_Backend.Application.Interfaces;
using HelseFlow_Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelseFlow_Backend.Infrastructure.Data;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<List<User>> GetAllPatientsAsync(string? search, string? region, int? ageMin, int? ageMax)
    {
        var query = _context.Users.Where(u => u.Role == "patient").AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(u => u.Name.Contains(search, StringComparison.OrdinalIgnoreCase) || u.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(region))
        {
            query = query.Where(u => u.Region != null && u.Region.Equals(region, StringComparison.OrdinalIgnoreCase));
        }

        if (ageMin.HasValue)
        {
            query = query.Where(u => u.Age.HasValue && u.Age >= ageMin.Value);
        }

        if (ageMax.HasValue)
        {
            query = query.Where(u => u.Age.HasValue && u.Age <= ageMax.Value);
        }

        return await query.ToListAsync();
    }
}
