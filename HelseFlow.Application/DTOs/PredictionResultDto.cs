namespace HelseFlow_Backend.Application.DTOs;

public class PredictionResultDto
{
    public Guid PatientId { get; set; }
    public DateTime PredictionDate { get; set; }
    public Dictionary<string, RiskScoreDto> RiskScores { get; set; } = new Dictionary<string, RiskScoreDto>();
    public List<string> Recommendations { get; set; } = new List<string>();
}
