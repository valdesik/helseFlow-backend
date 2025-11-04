using HelseFlow_Backend.Application.Interfaces;
using HelseFlow_Backend.Domain.Entities;
using System.Collections.Generic;

namespace HelseFlow_Backend.Infrastructure.Data;

public class DataFactory
{
    private readonly IPasswordHasher _passwordHasher;

    public DataFactory(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public List<User> CreateTestUsers()
    {
        return new List<User>
        {
            new User(
                Guid.NewGuid(),
                "John Doe",
                "user@example.com",
                _passwordHasher.HashPassword("yourpassword"),
                "patient",
                40,
                "Bergen",
                new List<string> { "Smoking", "High-stress job" }
            ),
            new User(
                Guid.NewGuid(),
                "Admin User",
                "admin@example.com",
                _passwordHasher.HashPassword("adminpassword"),
                "admin"
            ),
            new User(
                Guid.NewGuid(),
                "Anna Nilsen",
                "anna.nilsen@example.com",
                _passwordHasher.HashPassword("password123"),
                "patient",
                45,
                "Oslo",
                new List<string> { "None" }
            ),
            new User(
                Guid.NewGuid(),
                "Bjorn Hansen",
                "bjorn.hansen@example.com",
                _passwordHasher.HashPassword("password123"),
                "patient",
                62,
                "Bergen",
                new List<string> { "High Blood Pressure" }
            )
        };
    }

    public List<Doctor> CreateTestDoctors()
    {
        return new List<Doctor>
        {
            new Doctor(
                Guid.NewGuid(),
                "Dr. Ivar Finnes",
                "Cardiology",
                "Oslo University Hospital",
                "https://example.com/ivar.jpg",
                "Dr. Ivar Finnes is a highly experienced cardiologist..."
            ),
            new Doctor(
                Guid.NewGuid(),
                "Dr. Solveig Larsen",
                "General Practice",
                "Bergen Medical Center",
                "https://example.com/solveig.jpg",
                "Dr. Solveig Larsen specializes in general practice..."
            )
        };
    }

    public List<Guideline> CreateTestGuidelines()
    {
        return new List<Guideline>
        {
            new Guideline(
                Guid.NewGuid(),
                "National guidelines for diabetes management",
                "Endocrinology",
                "Comprehensive recommendations for the diagnosis and treatment of diabetes.",
                "https://www.helsedirektoratet.no/retningslinjer/diabetes"
            ),
            new Guideline(
                Guid.NewGuid(),
                "Guidelines for cardiovascular disease prevention",
                "Cardiology",
                "Recommendations for preventing heart and blood vessel diseases.",
                "https://www.helsedirektoratet.no/retningslinjer/cardio"
            )
        };
    }
}
