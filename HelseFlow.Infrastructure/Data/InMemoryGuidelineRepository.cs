using HelseFlow_Backend.Application.Interfaces;
using HelseFlow_Backend.Domain.Entities;

namespace HelseFlow_Backend.Infrastructure.Data;

public class InMemoryGuidelineRepository : IGuidelineRepository
{
    private readonly List<Guideline> _guidelines = new()
    {
        new Guideline(
            Guid.Parse("01a2b3c4-d5e6-7890-1234-567890abcdef"),
            "National guidelines for diabetes management",
            "Endocrinology",
            "Comprehensive recommendations for the diagnosis and treatment of diabetes.",
            "https://www.helsedirektoratet.no/retningslinjer/diabetes"
        ),
        new Guideline(
            Guid.Parse("02a3b4c5-d6e7-8901-2345-67890abcdef0"),
            "Guidelines for cardiovascular disease prevention",
            "Cardiology",
            "Recommendations for preventing heart and blood vessel diseases.",
            "https://www.helsedirektoratet.no/retningslinjer/cardio"
        )
    };

    public Task<List<Guideline>> GetAllAsync(string? category, string? search)
    {
        var query = _guidelines.AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(g => g.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(g => g.Title.Contains(search, StringComparison.OrdinalIgnoreCase) || g.Summary.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(query.ToList());
    }
}
