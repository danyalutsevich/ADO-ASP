namespace ASP.Models.Forum;

public class ForumSectionsModel
{
    public bool UserCanCreate { get; set; }
    public List<ForumThemeModel> Themes { get; set; } = null!;  

    public string CreateMessage { get; set; } = "";
    public bool IsMessagePositive { get; set; }
    public CreateThemeModel FormModel { get; set; } = null!;
}