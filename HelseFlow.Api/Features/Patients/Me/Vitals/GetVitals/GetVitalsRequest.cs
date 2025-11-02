namespace HelseFlow_Backend.Api.Features.Patients.Me.Vitals.GetVitals;

public class GetVitalsRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
