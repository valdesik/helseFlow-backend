using FastEndpoints;
using HelseFlow_Backend.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseFlow_Backend.Api.Features.Admin.Patients.UpdatePatientProfile;

public class UpdatePatientProfileEndpoint : Endpoint<UpdatePatientProfileRequest, UpdatePatientProfileResponse>
{
    private readonly AdminService _adminService;

    public UpdatePatientProfileEndpoint(AdminService adminService)
    {
        _adminService = adminService;
    }

    public override void Configure()
    {
        Put("/api/admin/patients/{Id}");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Roles("admin"); // Only administrators can update patient profiles
        Summary(s =>
        {
            s.Summary = "Updates a patient's profile by an administrator.";
            s.Description = "This endpoint allows administrators to modify a patient's profile.";

            s.Responses[200] = "Patient profile updated successfully.";
            s.Responses[400] = "Bad Request (e.g., invalid input or email already taken).";
            s.Responses[401] = "Unauthorized if no token or invalid token is provided.";
            s.Responses[403] = "Forbidden if the user is not an admin.";
            s.Responses[404] = "Patient not found.";
        });
    }

    public override async Task HandleAsync(UpdatePatientProfileRequest req, CancellationToken ct)
    {
        var updatedPatientDto = await _adminService.UpdatePatientProfileAsync(
            req.Id,
            req.Name,
            req.Email,
            req.Age,
            req.Region,
            req.RiskFactors
        );

        if (updatedPatientDto == null)
        {
            // Could be 404 (patient not found) or 400 (email already taken)
            // For simplicity, returning 400 for now, but in a real app, distinguish.
            await SendErrorsAsync(400, ct); 
            return;
        }

        await SendAsync(new UpdatePatientProfileResponse { Patient = updatedPatientDto }, cancellation: ct);
    }
}
