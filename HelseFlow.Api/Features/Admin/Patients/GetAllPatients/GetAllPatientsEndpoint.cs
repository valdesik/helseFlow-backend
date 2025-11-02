using FastEndpoints;
using HelseFlow_Backend.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseFlow_Backend.Api.Features.Admin.Patients.GetAllPatients;

public class GetAllPatientsEndpoint : Endpoint<GetAllPatientsRequest, GetAllPatientsResponse>
{
    private readonly AdminService _adminService;

    public GetAllPatientsEndpoint(AdminService adminService)
    {
        _adminService = adminService;
    }

    public override void Configure()
    {
        Get("/api/admin/patients");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Roles("admin"); // Only administrators can retrieve the list of patients
        Summary(s =>
        {
            s.Summary = "Retrieves a list of all registered patients.";
            s.Description = "This endpoint allows administrators to view a list of all patients, with optional filtering.";

            s.Responses[200] = "Successfully retrieved the list of patients.";
            s.Responses[401] = "Unauthorized if no token or invalid token is provided.";
            s.Responses[403] = "Forbidden if the user is not an admin.";
        });
    }

    public override async Task HandleAsync(GetAllPatientsRequest req, CancellationToken ct)
    {
        var patients = await _adminService.GetAllPatientsAsync(req.Search, req.Region, req.AgeMin, req.AgeMax);

        await SendAsync(new GetAllPatientsResponse { Patients = patients }, cancellation: ct);
    }
}
