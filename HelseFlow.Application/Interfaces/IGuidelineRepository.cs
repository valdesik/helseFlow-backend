using HelseFlow_Backend.Domain.Entities;

namespace HelseFlow_Backend.Application.Interfaces;

public interface IGuidelineRepository
{
    Task<List<Guideline>> GetAllAsync(string? category, string? search);
}
