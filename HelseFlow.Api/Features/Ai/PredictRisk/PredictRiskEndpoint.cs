using FastEndpoints;
using HelseFlow_Backend.Application.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseFlow_Backend.Api.Features.Ai.PredictRisk;

public class PredictRiskEndpoint : Endpoint<PredictRiskRequest, PredictRiskResponse>
{
    private readonly AiService _aiService;

    public PredictRiskEndpoint(AiService aiService)
    {
        _aiService = aiService;
    }

    public override void Configure()
    {
        Post("/api/ai/predict-risk");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Roles("patient", "admin"); // Patients and administrators can use this endpoint
        Summary(s =>
        {
            s.Summary = "Sends patient data to the ML model to predict health risks.";
            s.Description = "This endpoint interacts with an AI model to provide health risk predictions and recommendations.";

            s.Responses[200] = "Risk prediction successful.";
            s.Responses[400] = "Bad Request (e.g., invalid input).";
            s.Responses[401] = "Unauthorized if no token or invalid token is provided.";
            s.Responses[403] = "Forbidden if the user does not have the required role.";
        });
    }

    public override async Task HandleAsync(PredictRiskRequest req, CancellationToken ct)
    {
        Guid patientId;

        if (User.IsInRole("admin") && req.PatientId.HasValue)
        {
            // If PatientId is provided in the request, and the user is an admin, use it
            patientId = req.PatientId.Value;
        }
        else if (User.IsInRole("patient") && !req.PatientId.HasValue)
        {
            // If PatientId is not provided, and the user is a patient, use the current user's ID
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out patientId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }
        }
        else
        {
            // Invalid scenario (e.g., patient providing PatientId, or admin not providing it when needed)
            await SendForbiddenAsync(ct);
            return;
        }

        var predictionResult = await _aiService.PredictRiskAsync(
            patientId,
            req.Age,
            req.Gender,
            req.BloodPressureSystolic,
            req.BloodPressureDiastolic,
            req.Weight,
            req.RiskFactors
        );

        await SendAsync(new PredictRiskResponse { PredictionResult = predictionResult }, cancellation: ct);
    }
}
