using HelseFlow_Backend.Application.Interfaces;
using HelseFlow_Backend.Domain.Entities;

namespace HelseFlow_Backend.Infrastructure.Data;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public InMemoryUserRepository(IPasswordHasher passwordHasher)
    {
        // Add test users
        var testUser = new User(
            Guid.Parse("c927f8e5-2a3b-4c1d-8e0a-0b9c7d6e5f4a"),
            "John Doe",
            "user@example.com",
            passwordHasher.HashPassword("yourpassword"),
            "patient", // Role added
            40,
            "Bergen",
            new List<string> { "Smoking", "High-stress job" }
        );
        _users.Add(testUser);

        var adminUser = new User(
            Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef"),
            "Admin User",
            "admin@example.com",
            passwordHasher.HashPassword("adminpassword"),
            "admin" // Role added
            // Age, Region, RiskFactors are optional and can be omitted if not needed
        );
        _users.Add(adminUser);

        var annaNilsen = new User(
            Guid.Parse("a0a1a2a3-b4b5-c6c7-d8d9-e0e1e2e3e4e5"),
            "Anna Nilsen",
            "anna.nilsen@example.com",
            passwordHasher.HashPassword("password"),
            "patient", // Role added
            45,
            "Oslo",
            new List<string> { "None" }
        );
        _users.Add(annaNilsen);

        var bjornHansen = new User(
            Guid.Parse("f0f1f2f3-a4b5-c6d7-e8f9-0123456789ab"),
            "Bjorn Hansen",
            "bjorn.hansen@example.com",
            passwordHasher.HashPassword("password"),
            "patient", // Role added
            62,
            "Bergen",
            new List<string> { "High Blood Pressure" }
        );
        _users.Add(bjornHansen);
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));
    }

    public Task AddAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user)
    {
        var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            _users.Remove(existingUser);
            _users.Add(user);
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _users.RemoveAll(u => u.Id == id);
        return Task.CompletedTask;
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
    }

    public Task<List<User>> GetAllPatientsAsync(string? search, string? region, int? ageMin, int? ageMax)
    {
        var query = _users.Where(u => u.Role == "patient").AsQueryable();

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

        return Task.FromResult(query.ToList());
    }
}
