namespace HelseFlow_Backend.Domain.Entities;

public class Guideline
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Category { get; private set; }
    public string Summary { get; private set; }
    public string Link { get; private set; }

    private Guideline() { } // Private constructor for ORM

    public Guideline(Guid id, string title, string category, string summary, string link)
    {
        Id = id;
        Title = title;
        Category = category;
        Summary = summary;
        Link = link;
    }
}
