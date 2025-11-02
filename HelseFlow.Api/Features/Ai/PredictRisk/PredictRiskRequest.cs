namespace HelseFlow_Backend.Api.Features.Ai.PredictRisk;

public class PredictRiskRequest
{
    public Guid? PatientId { get; set; } // Optional, if admin makes the request for a specific patient
    public int Age { get; set; }
    public string Gender { get; set; }
    public int BloodPressureSystolic { get; set; }
    public int BloodPressureDiastolic { get; set; }
    public double Weight { get; set; }
    public List<string> RiskFactors { get; set; } = new List<string>();
}
