namespace HelseFlow_Backend.Api.Features.Patients.Me.Appointments.BookAppointment;

public class BookAppointmentRequest
{
    public Guid DoctorId { get; set; }
    public DateTime AppointmentTime { get; set; }
    public string Reason { get; set; }
}
