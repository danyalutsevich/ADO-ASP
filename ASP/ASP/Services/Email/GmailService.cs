using System.Net;
using System.Net.Mail;

namespace ASP.Services.Email;

public class GmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<GmailService> _logger;

    public GmailService(IConfiguration configuration, ILogger<GmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public bool Send(string templateName, object model)
    {
        if (templateName is null)
        {
            throw new ArgumentNullException(nameof(templateName));
        }

        if (model is null)
        {
            throw new ArgumentException(nameof(model));
        }

        string[] possibleTemplates = new string[]
        {
            templateName,
            $"{templateName}.html",
            "EmailTemplates/" + templateName,
            "EmailTemplates/" + templateName + ".html",
        };
        string template = null;

        foreach (string possibleTemplate in possibleTemplates)
        {
            if (File.Exists(possibleTemplate))
            {
                template = File.ReadAllText(possibleTemplate);
                break;
            }
        }

        if (template is null)
        {
            throw new FileNotFoundException("Template not found", templateName);
        }

        string userEmail = null;
        foreach (var prop in model.GetType().GetProperties())
        {
            if (prop.Name.ToLower() == "email")
            {
                userEmail = prop.GetValue(model).ToString();
            }

            string placeholder = $"{{{{{prop.Name}}}}}";
            template = template.Replace(placeholder, prop.GetValue(model).ToString() ?? "");
        }

        if (userEmail is null)
        {
            throw new ArgumentException("Model has no Email property");
        }

        string host = _configuration["SMTP:Host"];
        if (host is null)
        {
            throw new MissingFieldException("SMTP:Host property is not defined in appsettings.json");
        }

        string fromEmail = _configuration["SMTP:Email"];
        if (fromEmail is null)
        {
            throw new MissingFieldException("SMTP:Email property is not defined in appsettings.json");
        }

        string password = _configuration["SMTP:Password"];
        if (password is null)
        {
            throw new MissingFieldException("SMTP:Password property is not defined in appsettings.json");
        }

        int port = int.Parse(_configuration["SMTP:Port"]);

        bool ssl = bool.Parse(_configuration["SMTP:SSL"]);

        MailMessage message = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = "",
            IsBodyHtml = true,
            Body = template,
            To = { userEmail }
            
        };
        
        using SmtpClient smtp = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(fromEmail, password),
            EnableSsl = ssl,
        };
        try
        {
            smtp.Send(message);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error sending email");
            return false;
        }

        Console.WriteLine("Email sent");
        return true;
    }
}