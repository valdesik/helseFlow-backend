using FastEndpoints;
using HelseFlow_Backend.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseFlow_Backend.Api.Features.Admin.Patients.DeletePatient;

public class DeletePatientEndpoint : Endpoint<DeletePatientRequest>
{
    private readonly AdminService _adminService;

    public DeletePatientEndpoint(AdminService adminService)
    {
        _adminService = adminService;
    }

    public override void Configure()
    {
        Delete("/api/admin/patients/{Id}");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Roles("admin"); // Only administrators can delete patients
        Summary(s =>
        {
            s.Summary = "Deletes a patient and all associated data.";
            s.Description = "This endpoint allows administrators to permanently remove a patient record.";

            s.Responses[204] = "Patient deleted successfully.";
            s.Responses[401] = "Unauthorized if no token or invalid token is provided.";
            s.Responses[403] = "Forbidden if the user is not an admin.";
            s.Responses[404] = "Patient not found.";
        });
    }

    public override async Task HandleAsync(DeletePatientRequest req, CancellationToken ct)
    {
        var result = await _adminService.DeletePatientAsync(req.Id);

        if (!result)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendNoContentAsync(ct);
    }
}
