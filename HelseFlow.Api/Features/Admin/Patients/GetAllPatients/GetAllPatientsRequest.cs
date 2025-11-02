namespace HelseFlow_Backend.Api.Features.Admin.Patients.GetAllPatients;

public class GetAllPatientsRequest
{
    public string? Search { get; set; }
    public string? Region { get; set; }
    public int? AgeMin { get; set; }
    public int? AgeMax { get; set; }
}
