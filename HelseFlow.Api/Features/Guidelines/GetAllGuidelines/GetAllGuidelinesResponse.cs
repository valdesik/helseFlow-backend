using HelseFlow_Backend.Application.DTOs;

namespace HelseFlow_Backend.Api.Features.Guidelines.GetAllGuidelines;

public class GetAllGuidelinesResponse
{
    public List<GuidelineDto> Guidelines { get; set; }
}
