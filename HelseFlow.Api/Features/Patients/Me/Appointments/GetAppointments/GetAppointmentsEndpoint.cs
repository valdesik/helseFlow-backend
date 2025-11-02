using FastEndpoints;
using HelseFlow_Backend.Application.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseFlow_Backend.Api.Features.Patients.Me.Appointments.GetAppointments;

public class GetAppointmentsEndpoint : Endpoint<GetAppointmentsRequest, GetAppointmentsResponse>
{
    private readonly PatientService _patientService;

    public GetAppointmentsEndpoint(PatientService patientService)
    {
        _patientService = patientService;
    }

    public override void Configure()
    {
        Get("/api/patients/me/appointments");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Roles("patient"); // Only patients can retrieve their appointments
        Summary(s =>
        {
            s.Summary = "Retrieves all appointments for the authenticated patient.";
            s.Description = "This endpoint allows an authenticated patient to view their appointments, optionally filtered by status.";

            s.Responses[200] = "Successfully retrieved appointments.";
            s.Responses[401] = "Unauthorized if no token or invalid token is provided.";
            s.Responses[403] = "Forbidden if the user is not a patient.";
        });
    }

    public override async Task HandleAsync(GetAppointmentsRequest req, CancellationToken ct)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var patientId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var appointments = await _patientService.GetPatientAppointmentsAsync(patientId, req.Status);

        await SendAsync(new GetAppointmentsResponse { Appointments = appointments }, cancellation: ct);
    }
}
