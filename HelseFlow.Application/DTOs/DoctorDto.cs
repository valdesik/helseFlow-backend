namespace HelseFlow_Backend.Application.DTOs;

public class DoctorDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Specialty { get; set; }
    public string Location { get; set; }
    public string? ImageUrl { get; set; }
    public string? Bio { get; set; }
}
