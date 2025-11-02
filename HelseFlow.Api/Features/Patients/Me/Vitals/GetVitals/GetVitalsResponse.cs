using HelseFlow_Backend.Application.DTOs;

namespace HelseFlow_Backend.Api.Features.Patients.Me.Vitals.GetVitals;

public class GetVitalsResponse
{
    public List<VitalLogDto> VitalLogs { get; set; }
}
