namespace HelseFlow_Backend.Application.DTOs;

public class RiskScoreDto
{
    public string Level { get; set; }
    public double Probability { get; set; }
    public double Threshold { get; set; }
}
