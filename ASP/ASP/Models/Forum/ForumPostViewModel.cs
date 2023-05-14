namespace ASP.Models.Forum;

public class ForumPostViewModel
{
    public string Content { get; set; } = "";
    public string CreatedDt { get; set; }
    public string AuthorAvatar { get; set; } = "";
    public string AuthorName { get; set; } = "";
    public string ReplyPreview { get; set; } = "";
}