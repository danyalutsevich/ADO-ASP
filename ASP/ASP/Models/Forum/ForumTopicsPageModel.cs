namespace ASP.Models.Forum;

public class ForumTopicsPageModel
{
    public bool UserCanCreate { get; set; }
    public List<ForumPostViewModel> Posts { get; set; } = null!;
    public string Title { get; set; } = "";
    public string TopicId { get; set; } = "";
    public string Description { get; set; } = "";
    public string CreateMessage { get; set; } = "";
    public bool IsMessagePositive { get; set; }
    public ForumTopicFormModel FormModel { get; set; } = null!;
}