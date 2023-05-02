namespace ASP.Data.Entity;

public class Topic
{
    public Guid Id { get; set; }
    public Guid ThemeId { get; set; }
    public String Title { get; set; } = null!;
    public String Description { get; set; } = null!;
    public Guid AuthorId { get; set; }
    public DateTime CreatedDt { get; set; }
}