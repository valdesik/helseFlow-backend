using FastEndpoints;
using HelseFlow_Backend.Api.Features.Auth.Login; // Corrected namespace
using HelseFlow_Backend.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseFlow_Backend.Api.Features.Auth.Login;

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    private readonly AuthService _authService;

    public LoginEndpoint(AuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("/api/auth/login");
        AllowAnonymous(); // This endpoint does not require authentication
        Tags("Auth"); // Group this endpoint under the "Auth" tag
        Summary(s =>
        {
            s.Summary = "Authenticates a user and returns a JWT token and user details.";
            s.Description = "This endpoint handles user login by validating credentials and issuing a JWT.";

            s.Responses[200] = "Authentication successful, returns token and user info.";
            s.Responses[401] = "Invalid credentials.";
        });
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var result = await _authService.LoginAsync(req.Email, req.Password);

        if (result == null)
        {
            await SendUnauthorizedAsync(ct); // Return 401 Unauthorized
            return;
        }

        await SendAsync(new LoginResponse { Token = result.Token, User = result.User }, cancellation: ct);
    }
}
