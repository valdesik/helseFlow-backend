namespace HelseFlow_Backend.Api.Features.Admin.Analytics.Regional;

public class GetRegionalHealthStatsRequest
{
    public string? Region { get; set; }
    public string? AgeGroup { get; set; }
    public string? Gender { get; set; }
}
