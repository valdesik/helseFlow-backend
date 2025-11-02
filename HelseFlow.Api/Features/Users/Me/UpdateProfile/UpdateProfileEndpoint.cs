using System.Security.Claims;
using FastEndpoints;
using HelseFlow_Backend.Api.Features.Users.Me.UpdateProfile;
using HelseFlow_Backend.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseFlow_Backend.HelseFlow.Api.Features.Users.Me.UpdateProfile;

public class UpdateProfileEndpoint : Endpoint<UpdateProfileRequest, UpdateProfileResponse>
{
    private readonly AuthService _authService;

    public UpdateProfileEndpoint(AuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Put("/api/users/me");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme); // Requires JWT authentication
        Summary(s =>
        {
            s.Summary = "Updates the profile of the currently authenticated user.";
            s.Description = "This endpoint allows an authenticated user to update their profile information.";

            s.Responses[200] = "Profile updated successfully.";
            s.Responses[400] = "Bad Request (e.g., invalid input).";
            s.Responses[401] = "Unauthorized if no token or invalid token is provided.";
            s.Responses[404] = "User not found (should not happen for authenticated users).";
        });
    }

    public override async Task HandleAsync(UpdateProfileRequest req, CancellationToken ct)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var updatedUserDto = await _authService.UpdateProfileAsync(
            userId,
            req.Name,
            req.Age,
            req.Region,
            req.RiskFactors
        );

        if (updatedUserDto == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(new UpdateProfileResponse { User = updatedUserDto }, cancellation: ct);
    }
}
