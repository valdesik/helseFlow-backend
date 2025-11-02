namespace HelseFlow_Backend.Domain.Entities;

public class VitalLog
{
    public Guid Id { get; private set; }
    public Guid PatientId { get; private set; }
    public DateTime Date { get; private set; }
    public int BloodPressureSystolic { get; private set; }
    public int BloodPressureDiastolic { get; private set; }
    public int Pulse { get; private set; }
    public double Weight { get; private set; }
    public string Mood { get; private set; }

    private VitalLog() { } // Private constructor for ORM

    public VitalLog(Guid id, Guid patientId, DateTime date, int bloodPressureSystolic, int bloodPressureDiastolic, int pulse, double weight, string mood)
    {
        Id = id;
        PatientId = patientId;
        Date = date;
        BloodPressureSystolic = bloodPressureSystolic;
        BloodPressureDiastolic = bloodPressureDiastolic;
        Pulse = pulse;
        Weight = weight;
        Mood = mood;
    }
}
