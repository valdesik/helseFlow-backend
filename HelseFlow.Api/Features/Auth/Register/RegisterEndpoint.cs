using FastEndpoints;
using HelseFlow_Backend.Api.Features.Auth.Register;
using HelseFlow_Backend.Application.Services;

namespace HelseFlow_Backend.HelseFlow.Api.Features.Auth.Register;

public class RegisterEndpoint : Endpoint<RegisterRequest, RegisterResponse>
{
    private readonly AuthService _authService;

    public RegisterEndpoint(AuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("/api/auth/register");
        AllowAnonymous(); // This endpoint does not require authentication
        Summary(s =>
        {
            s.Summary = "Registers a new patient user.";
            s.Description = "This endpoint allows new users to register as patients.";

            s.Responses[201] = "Registration successful, returns user info.";
            s.Responses[400] = "Bad Request (e.g., invalid input).";
            s.Responses[409] = "Email already registered.";
        });
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        var result = await _authService.RegisterAsync(
            req.Name,
            req.Email,
            req.Password,
            req.Age,
            req.Region,
            req.RiskFactors
        );

        if (result == null)
        {
            // Email already registered
            await SendErrorsAsync(409, ct); // 409 Conflict
            return;
        }

        await SendAsync(new RegisterResponse { User = result.User }, statusCode: StatusCodes.Status201Created, cancellation: ct);
    }
}
