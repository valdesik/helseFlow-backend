using HelseFlow_Backend.Application.DTOs;
using HelseFlow_Backend.Application.Interfaces;

namespace HelseFlow_Backend.Application.Services;

public class AiService
{
    private readonly IUserRepository _userRepository;

    public AiService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PredictionResultDto> PredictRiskAsync(
        Guid patientId,
        int age,
        string gender,
        int bloodPressureSystolic,
        int bloodPressureDiastolic,
        double weight,
        List<string> riskFactors)
    {
        // Stub for ML model
        // In a real application, this would involve calling an ML model

        var riskScores = new Dictionary<string, RiskScoreDto>();
        var recommendations = new List<string>();

        // Example prediction logic
        if (bloodPressureSystolic > 130 || bloodPressureDiastolic > 85)
        {
            riskScores.Add("hypertension", new RiskScoreDto { Level = "High", Probability = 0.75, Threshold = 0.70 });
            recommendations.Add("Based on your measurements, it is recommended to consult a doctor regarding your blood pressure.");
            recommendations.Add("Consider reducing salt intake as per Helsedirektoratet guidelines.");
        }
        else
        {
            riskScores.Add("hypertension", new RiskScoreDto { Level = "Low", Probability = 0.15, Threshold = 0.70 });
        }

        if (riskFactors.Contains("Family History") && age > 30)
        {
            riskScores.Add("diabetes", new RiskScoreDto { Level = "Medium", Probability = 0.40, Threshold = 0.20 });
            recommendations.Add("Given your family history and age, regular blood sugar checks are advised.");
        }
        else
        {
            riskScores.Add("diabetes", new RiskScoreDto { Level = "Low", Probability = 0.15, Threshold = 0.20 });
        }

        return new PredictionResultDto
        {
            PatientId = patientId,
            PredictionDate = DateTime.UtcNow,
            RiskScores = riskScores,
            Recommendations = recommendations
        };
    }
}
