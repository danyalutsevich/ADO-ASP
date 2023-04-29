namespace ASP.Services.Email;

public interface IEmailService
{
    bool Send(string templateName, object model);
}