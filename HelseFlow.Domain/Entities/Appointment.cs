namespace HelseFlow_Backend.Domain.Entities;

public class Appointment
{
    public Guid Id { get; private set; }
    public Guid PatientId { get; private set; }
    public Guid DoctorId { get; private set; }
    public DateTime AppointmentTime { get; private set; }
    public string Reason { get; private set; }
    public string Status { get; private set; } // e.g., "Confirmed", "Pending", "Cancelled", "Completed"

    private Appointment() { } // Private constructor for ORM

    public Appointment(Guid id, Guid patientId, Guid doctorId, DateTime appointmentTime, string reason, string status)
    {
        Id = id;
        PatientId = patientId;
        DoctorId = doctorId;
        AppointmentTime = appointmentTime;
        Reason = reason;
        Status = status;
    }

    public void Confirm()
    {
        Status = "Confirmed";
    }

    public void Cancel()
    {
        Status = "Cancelled";
    }

    public void Complete()
    {
        Status = "Completed";
    }
}
