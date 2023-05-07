using ASP.Data.Entity;

namespace ASP.Models.Forum;

public class ForumIndexModel
{
    public bool UserCanCreate { get; set; }
    public List<ForumSectionModel> Sections { get; set; } = null!;
    public string CreateMessage { get; set; } = "";
    public bool IsMessagePositive { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
}