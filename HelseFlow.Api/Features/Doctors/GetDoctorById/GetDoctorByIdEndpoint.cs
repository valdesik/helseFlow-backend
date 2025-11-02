using FastEndpoints;
using HelseFlow_Backend.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseFlow_Backend.Api.Features.Doctors.GetDoctorById;

public class GetDoctorByIdEndpoint : Endpoint<GetDoctorByIdRequest, GetDoctorByIdResponse>
{
    private readonly PatientService _patientService;

    public GetDoctorByIdEndpoint(PatientService patientService)
    {
        _patientService = patientService;
    }

    public override void Configure()
    {
        Get("/api/doctors/{Id}");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Roles("patient"); // Only patients can view doctor profiles
        Summary(s =>
        {
            s.Summary = "Retrieves detailed information for a specific doctor.";
            s.Description = "This endpoint allows an authenticated patient to view the detailed profile of a doctor by their ID.";

            s.Responses[200] = "Successfully retrieved doctor's profile.";
            s.Responses[401] = "Unauthorized if no token or invalid token is provided.";
            s.Responses[403] = "Forbidden if the user is not a patient.";
            s.Responses[404] = "Doctor not found.";
        });
    }

    public override async Task HandleAsync(GetDoctorByIdRequest req, CancellationToken ct)
    {
        var doctor = await _patientService.GetDoctorByIdAsync(req.Id);

        if (doctor == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(new GetDoctorByIdResponse { Doctor = doctor }, cancellation: ct);
    }
}
