using FastEndpoints;
using HelseFlow_Backend.Application.DTOs;
using HelseFlow_Backend.Application.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseFlow_Backend.Api.Features.Users.Me;

// For GET requests that do not have a body, EmptyRequest can be used
public class GetProfileEndpoint : EndpointWithoutRequest<UserDto>
{
    private readonly IUserRepository _userRepository;

    public GetProfileEndpoint(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override void Configure()
    {
        Get("/api/users/me");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme); // Requires JWT authentication
        Summary(s =>
        {
            s.Summary = "Retrieves the profile of the currently authenticated user.";
            s.Description = "This endpoint returns the detailed profile information for the user whose JWT token is provided.";

            s.Responses[200] = "Successfully retrieved user profile.";
            s.Responses[401] = "Unauthorized if no token or invalid token is provided.";
            s.Responses[404] = "User not found (should not happen for authenticated users).";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        // Get user ID from JWT token
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            // This should not happen if authentication was successful,
            // but for security, we check.
            await SendUnauthorizedAsync(ct);
            return;
        }

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            // Again, unlikely for an authenticated user,
            // but better to handle.
            await SendNotFoundAsync(ct);
            return;
        }

        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Age = user.Age,
            Region = user.Region,
            RiskFactors = user.RiskFactors
        };

        await SendAsync(userDto, cancellation: ct);
    }
}
