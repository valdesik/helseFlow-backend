namespace HelseFlow_Backend.Application.DTOs;

public class VitalLogDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public DateTime Date { get; set; }
    public int BloodPressureSystolic { get; set; }
    public int BloodPressureDiastolic { get; set; }
    public int Pulse { get; set; }
    public double Weight { get; set; }
    public string Mood { get; set; }
}
