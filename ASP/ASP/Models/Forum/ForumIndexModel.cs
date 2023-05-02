using ASP.Data.Entity;

namespace ASP.Models.Forum;

public class ForumIndexModel
{
    public bool UserCanCreate { get; set; }
    public List<Section> Sections { get; set; } = null!;
    public string CreateMessage { get; set; } = "";
}