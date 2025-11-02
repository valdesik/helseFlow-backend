using FastEndpoints;
using HelseFlow_Backend.Application.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace HelseFlow_Backend.Api.Features.Patients.Me.Vitals.LogVitals;

public class LogVitalsEndpoint : Endpoint<LogVitalsRequest, LogVitalsResponse>
{
    private readonly PatientService _patientService;

    public LogVitalsEndpoint(PatientService patientService)
    {
        _patientService = patientService;
    }

    public override void Configure()
    {
        Post("/api/patients/me/vitals");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Roles("patient"); // Only patients can log vitals
        Summary(s =>
        {
            s.Summary = "Records new vital signs for the authenticated patient.";
            s.Description = "This endpoint allows an authenticated patient to log their vital signs.";

            s.Responses[201] = "Vital signs logged successfully.";
            s.Responses[400] = "Bad Request (e.g., invalid input).";
            s.Responses[401] = "Unauthorized if no token or invalid token is provided.";
            s.Responses[403] = "Forbidden if the user is not a patient.";
        });
    }

    public override async Task HandleAsync(LogVitalsRequest req, CancellationToken ct)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var patientId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var vitalLogDto = await _patientService.LogVitalsAsync(
            patientId,
            req.Date,
            req.BloodPressureSystolic,
            req.BloodPressureDiastolic,
            req.Pulse,
            req.Weight,
            req.Mood
        );

        await SendAsync(new LogVitalsResponse { VitalLog = vitalLogDto }, statusCode: StatusCodes.Status201Created, cancellation: ct);
    }
}
