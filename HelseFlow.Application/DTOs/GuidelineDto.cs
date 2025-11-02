namespace HelseFlow_Backend.Application.DTOs;

public class GuidelineDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Category { get; set; }
    public string Summary { get; set; }
    public string Link { get; set; }
}
