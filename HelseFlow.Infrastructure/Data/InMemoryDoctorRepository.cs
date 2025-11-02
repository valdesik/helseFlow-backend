using HelseFlow_Backend.Application.Interfaces;
using HelseFlow_Backend.Domain.Entities;

namespace HelseFlow_Backend.Infrastructure.Data;

public class InMemoryDoctorRepository : IDoctorRepository
{
    private readonly List<Doctor> _doctors = new()
    {
        new Doctor(
            Guid.Parse("d1c2b3a4-e5f6-7890-1234-567890abcdef"),
            "Dr. Ivar Finnes",
            "Cardiology",
            "Oslo University Hospital",
            "https://example.com/ivar.jpg",
            "Dr. Ivar Finnes is a highly experienced cardiologist..."
        ),
        new Doctor(
            Guid.Parse("f1e2d3c4-b5a6-0987-6543-210fedcba987"),
            "Dr. Solveig Larsen",
            "General Practice",
            "Bergen Medical Center",
            "https://example.com/solveig.jpg",
            "Dr. Solveig Larsen specializes in general practice..."
        )
    };

    public Task<List<Doctor>> GetAllAsync(string? specialty, string? search)
    {
        var query = _doctors.AsQueryable();

        if (!string.IsNullOrWhiteSpace(specialty))
        {
            query = query.Where(d => d.Specialty.Equals(specialty, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(d => d.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(query.ToList());
    }

    public Task<Doctor?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_doctors.FirstOrDefault(d => d.Id == id));
    }
}
