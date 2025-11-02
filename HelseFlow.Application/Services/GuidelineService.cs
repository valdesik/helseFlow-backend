using HelseFlow_Backend.Application.DTOs;
using HelseFlow_Backend.Application.Interfaces;

namespace HelseFlow_Backend.Application.Services;

public class GuidelineService
{
    private readonly IGuidelineRepository _guidelineRepository;

    public GuidelineService(IGuidelineRepository guidelineRepository)
    {
        _guidelineRepository = guidelineRepository;
    }

    public async Task<List<GuidelineDto>> GetAllGuidelinesAsync(string? category, string? search)
    {
        var guidelines = await _guidelineRepository.GetAllAsync(category, search);
        return guidelines.Select(g => new GuidelineDto
        {
            Id = g.Id,
            Title = g.Title,
            Category = g.Category,
            Summary = g.Summary,
            Link = g.Link
        }).ToList();
    }
}
