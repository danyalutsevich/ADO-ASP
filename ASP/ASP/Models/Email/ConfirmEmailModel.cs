namespace ASP.Models.Email;

public class ConfirmEmailModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string EmailCode { get; set; }
    public string ConfirmURL { get; set; }
}