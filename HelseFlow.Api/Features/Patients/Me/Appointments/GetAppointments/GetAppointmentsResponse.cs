using HelseFlow_Backend.Application.DTOs;

namespace HelseFlow_Backend.Api.Features.Patients.Me.Appointments.GetAppointments;

public class GetAppointmentsResponse
{
    public List<AppointmentDto> Appointments { get; set; }
}
