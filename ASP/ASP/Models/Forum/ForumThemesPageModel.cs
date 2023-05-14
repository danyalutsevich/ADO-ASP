namespace ASP.Models.Forum;

public class ForumThemesPageModel
{
    public bool UserCanCreate { get; set; }
    public List<ForumTopicViewModel> Topics { get; set; } = null!;
    public string Title { get; set; } = "";
    public string ThemeId { get; set; } = "";
    public string CreateMessage { get; set; } = "";
    public bool IsMessagePositive { get; set; }
    public ForumTopicFormModel FormModel { get; set; } = null!;
    public string LogoId { get; set; } = "";
}