using FastEndpoints;
using HelseFlow_Backend.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseFlow_Backend.Api.Features.Admin.Analytics.Regional;

public class GetRegionalHealthStatsEndpoint : Endpoint<GetRegionalHealthStatsRequest, GetRegionalHealthStatsResponse>
{
    private readonly AdminService _adminService;

    public GetRegionalHealthStatsEndpoint(AdminService adminService)
    {
        _adminService = adminService;
    }

    public override void Configure()
    {
        Get("/api/admin/analytics/regional");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Roles("admin"); // Only administrators have access to analytics
        Summary(s =>
        {
            s.Summary = "Aggregates health statistics by region.";
            s.Description = "This endpoint provides aggregated health data for administrators, with optional filtering.";

            s.Responses[200] = "Successfully retrieved regional health statistics.";
            s.Responses[401] = "Unauthorized if no token or invalid token is provided.";
            s.Responses[403] = "Forbidden if the user is not an admin.";
        });
    }

    public override async Task HandleAsync(GetRegionalHealthStatsRequest req, CancellationToken ct)
    {
        var stats = await _adminService.GetRegionalHealthStatisticsAsync(req.Region, req.AgeGroup, req.Gender);

        await SendAsync(new GetRegionalHealthStatsResponse { Statistics = stats }, cancellation: ct);
    }
}
