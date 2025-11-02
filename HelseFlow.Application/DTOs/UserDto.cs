namespace HelseFlow_Backend.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public int? Age { get; set; }
    public string? Region { get; set; }
    public List<string>? RiskFactors { get; set; }
}
