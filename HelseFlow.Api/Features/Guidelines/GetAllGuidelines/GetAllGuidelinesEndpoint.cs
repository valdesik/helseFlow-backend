using FastEndpoints;
using HelseFlow_Backend.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseFlow_Backend.Api.Features.Guidelines.GetAllGuidelines;

public class GetAllGuidelinesEndpoint : Endpoint<GetAllGuidelinesRequest, GetAllGuidelinesResponse>
{
    private readonly GuidelineService _guidelineService;

    public GetAllGuidelinesEndpoint(GuidelineService guidelineService)
    {
        _guidelineService = guidelineService;
    }

    public override void Configure()
    {
        Get("/api/guidelines");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        // Roles("patient", "admin"); // Any authenticated user
        Summary(s =>
        {
            s.Summary = "Retrieves a list of national health guidelines.";
            s.Description = "This endpoint provides access to health guidelines, optionally filtered by category or search term.";

            s.Responses[200] = "Successfully retrieved guidelines.";
            s.Responses[401] = "Unauthorized if no token or invalid token is provided.";
        });
    }

    public override async Task HandleAsync(GetAllGuidelinesRequest req, CancellationToken ct)
    {
        var guidelines = await _guidelineService.GetAllGuidelinesAsync(req.Category, req.Search);

        await SendAsync(new GetAllGuidelinesResponse { Guidelines = guidelines }, cancellation: ct);
    }
}
