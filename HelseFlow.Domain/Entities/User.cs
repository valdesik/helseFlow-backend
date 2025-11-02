namespace HelseFlow_Backend.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; } // "patient" or "admin"
    public int? Age { get; private set; }
    public string? Region { get; private set; }
    public List<string>? RiskFactors { get; private set; } = new List<string>();

    // Private constructor for ORM or factory methods
    private User() { }

    public User(Guid id, string name, string email, string passwordHash, string role, int? age = null, string? region = null, List<string>? riskFactors = null)
    {
        Id = id;
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        Age = age;
        Region = region;
        RiskFactors = riskFactors ?? new List<string>();
    }

    public void SetPasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    // Method to update profile (will be used later)
    public void UpdateProfile(string name, int? age, string? region, List<string>? riskFactors)
    {
        Name = name;
        Age = age;
        Region = region;
        RiskFactors = riskFactors ?? new List<string>();
    }
}
