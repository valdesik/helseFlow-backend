namespace HelseFlow_Backend.Api.Features.Patients.Me.Vitals.LogVitals;

public class LogVitalsRequest
{
    public DateTime Date { get; set; }
    public int BloodPressureSystolic { get; set; }
    public int BloodPressureDiastolic { get; set; }
    public int Pulse { get; set; }
    public double Weight { get; set; }
    public string Mood { get; set; }
}
