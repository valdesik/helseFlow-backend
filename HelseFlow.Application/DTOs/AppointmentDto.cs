namespace HelseFlow_Backend.Application.DTOs;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public string DoctorName { get; set; } // Added for display convenience
    public DateTime AppointmentTime { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; }
}
