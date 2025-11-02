namespace HelseFlow_Backend.Api.Features.Users.Me.UpdateProfile;

public class UpdateProfileRequest
{
    public string Name { get; set; }
    public int? Age { get; set; }
    public string? Region { get; set; }
    public List<string>? RiskFactors { get; set; } = new List<string>();
}
