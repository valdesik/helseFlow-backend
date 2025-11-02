using FastEndpoints;
using HelseFlow_Backend.Application.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseFlow_Backend.Api.Features.Patients.Me.Vitals.GetVitals;

public class GetVitalsEndpoint : Endpoint<GetVitalsRequest, GetVitalsResponse>
{
    private readonly PatientService _patientService;

    public GetVitalsEndpoint(PatientService patientService)
    {
        _patientService = patientService;
    }

    public override void Configure()
    {
        Get("/api/patients/me/vitals");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Roles("patient"); // Only patients can retrieve their vitals
        Summary(s =>
        {
            s.Summary = "Retrieves all vital logs for the authenticated patient, optionally filtered by date range.";
            s.Description = "This endpoint allows an authenticated patient to retrieve their vital signs history.";

            s.Responses[200] = "Successfully retrieved vital logs.";
            s.Responses[401] = "Unauthorized if no token or invalid token is provided.";
            s.Responses[403] = "Forbidden if the user is not a patient.";
        });
    }

    public override async Task HandleAsync(GetVitalsRequest req, CancellationToken ct)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var patientId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var vitalLogs = await _patientService.GetPatientVitalLogsAsync(patientId, req.StartDate, req.EndDate);

        await SendAsync(new GetVitalsResponse { VitalLogs = vitalLogs }, cancellation: ct);
    }
}
