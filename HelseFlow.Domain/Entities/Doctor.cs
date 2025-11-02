namespace HelseFlow_Backend.Domain.Entities;

public class Doctor
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Specialty { get; private set; }
    public string Location { get; private set; }
    public string? ImageUrl { get; private set; }
    public string? Bio { get; private set; }

    private Doctor() { } // Private constructor for ORM

    public Doctor(Guid id, string name, string specialty, string location, string? imageUrl = null, string? bio = null)
    {
        Id = id;
        Name = name;
        Specialty = specialty;
        Location = location;
        ImageUrl = imageUrl;
        Bio = bio;
    }
}
