using FastEndpoints;
using HelseFlow_Backend.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseFlow_Backend.Api.Features.Admin.Patients.GetPatientDetails;

public class GetPatientDetailsEndpoint : Endpoint<GetPatientDetailsRequest, GetPatientDetailsResponse>
{
    private readonly AdminService _adminService;

    public GetPatientDetailsEndpoint(AdminService adminService)
    {
        _adminService = adminService;
    }

    public override void Configure()
    {
        Get("/api/admin/patients/{Id}");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Roles("admin"); // Only administrators can retrieve patient details
        Summary(s =>
        {
            s.Summary = "Retrieves detailed profile and all vital logs/appointments for a specific patient.";
            s.Description = "This endpoint allows administrators to view a patient's full record.";

            s.Responses[200] = "Successfully retrieved patient details.";
            s.Responses[401] = "Unauthorized if no token or invalid token is provided.";
            s.Responses[403] = "Forbidden if the user is not an admin.";
            s.Responses[404] = "Patient not found.";
        });
    }

    public override async Task HandleAsync(GetPatientDetailsRequest req, CancellationToken ct)
    {
        var patientDetails = await _adminService.GetPatientDetailsAsync(req.Id);

        if (patientDetails == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(new GetPatientDetailsResponse { PatientDetails = patientDetails }, cancellation: ct);
    }
}
