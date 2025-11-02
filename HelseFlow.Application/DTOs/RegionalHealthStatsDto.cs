namespace HelseFlow_Backend.Application.DTOs;

public class RegionalHealthStatsDto
{
    public string Region { get; set; }
    public double AvgBloodPressureSystolic { get; set; }
    public double AvgBloodPressureDiastolic { get; set; }
    public int PatientsAtRiskHypertension { get; set; }
    public int TotalPatients { get; set; }
}
