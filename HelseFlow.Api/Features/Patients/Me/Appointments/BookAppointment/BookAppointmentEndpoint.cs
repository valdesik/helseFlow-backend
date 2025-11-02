using FastEndpoints;
using HelseFlow_Backend.Application.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseFlow_Backend.Api.Features.Patients.Me.Appointments.BookAppointment;

public class BookAppointmentEndpoint : Endpoint<BookAppointmentRequest, BookAppointmentResponse>
{
    private readonly PatientService _patientService;

    public BookAppointmentEndpoint(PatientService patientService)
    {
        _patientService = patientService;
    }

    public override void Configure()
    {
        Post("/api/patients/me/appointments");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Roles("patient"); // Only patients can book appointments
        Summary(s =>
        {
            s.Summary = "Books an appointment with a specified doctor for the authenticated patient.";
            s.Description = "This endpoint allows an authenticated patient to book an appointment.";

            s.Responses[201] = "Appointment booked successfully.";
            s.Responses[400] = "Bad Request (e.g., invalid input or slot not available).";
            s.Responses[401] = "Unauthorized if no token or invalid token is provided.";
            s.Responses[403] = "Forbidden if the user is not a patient.";
            s.Responses[409] = "Appointment slot not available.";
        });
    }

    public override async Task HandleAsync(BookAppointmentRequest req, CancellationToken ct)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var patientId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var appointmentDto = await _patientService.BookAppointmentAsync(
            patientId,
            req.DoctorId,
            req.AppointmentTime,
            req.Reason
        );

        if (appointmentDto == null)
        {
            await SendErrorsAsync(409, ct); // 409 Conflict - Appointment slot not available or doctor not found
            return;
        }

        await SendAsync(new BookAppointmentResponse { Appointment = appointmentDto }, statusCode: StatusCodes.Status201Created, cancellation: ct);
    }
}
