using FastEndpoints;
using HelseFlow_Backend.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseFlow_Backend.Api.Features.Doctors.GetAllDoctors;

public class GetAllDoctorsEndpoint : Endpoint<GetAllDoctorsRequest, GetAllDoctorsResponse>
{
    private readonly PatientService _patientService;

    public GetAllDoctorsEndpoint(PatientService patientService)
    {
        _patientService = patientService;
    }

    public override void Configure()
    {
        Get("/api/doctors");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Roles("patient"); // Only patients can view doctors
        Summary(s =>
        {
            s.Summary = "Retrieves a list of all available doctors.";
            s.Description = "This endpoint allows authenticated patients to view a list of doctors, optionally filtered by specialty or name.";

            s.Responses[200] = "Successfully retrieved the list of doctors.";
            s.Responses[401] = "Unauthorized if no token or invalid token is provided.";
            s.Responses[403] = "Forbidden if the user is not a patient.";
        });
    }

    public override async Task HandleAsync(GetAllDoctorsRequest req, CancellationToken ct)
    {
        var doctors = await _patientService.GetAllDoctorsAsync(req.Specialty, req.Search);

        await SendAsync(new GetAllDoctorsResponse { Doctors = doctors }, cancellation: ct);
    }
}
