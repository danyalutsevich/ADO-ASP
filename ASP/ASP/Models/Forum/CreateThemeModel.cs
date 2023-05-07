using ASP.Data.Entity;

namespace ASP.Models.Forum;

public class CreateThemeModel
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public Guid SectionId { get; set; }
}