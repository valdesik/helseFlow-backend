using HelseFlow_Backend.Application.DTOs;

namespace HelseFlow_Backend.Api.Features.Admin.Analytics.Regional;

public class GetRegionalHealthStatsResponse
{
    public List<RegionalHealthStatsDto> Statistics { get; set; }
}
