namespace HelseFlow_Backend.Api.Features.Admin.Patients.UpdatePatientProfile;

public class UpdatePatientProfileRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int? Age { get; set; }
    public string? Region { get; set; }
    public List<string>? RiskFactors { get; set; } = new List<string>();
}
