namespace ASP.Data.Entity;

public class Theme
{
    public Guid Id { get; set; }
    public Guid SectionId { get; set; }
    public String Title { get; set; } = null!;
    public String Description { get; set; } = null!;
    public String LogoId { get; set; } = null!;
    public Guid AuthorId { get; set; }
    public DateTime CreatedDt { get; set; }
    
    public User Author { get; set; } = null!;
}