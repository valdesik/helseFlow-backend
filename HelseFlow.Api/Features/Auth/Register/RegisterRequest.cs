namespace HelseFlow_Backend.Api.Features.Auth.Register;

public class RegisterRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int? Age { get; set; }
    public string? Region { get; set; }
    public List<string>? RiskFactors { get; set; } = new List<string>();
}
