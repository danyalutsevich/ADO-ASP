using System.Security.Claims;
using ASP.Data;
using ASP.Models.Forum;
using ASP.Services.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.Controllers;

public class ForumController : Controller
{
    private readonly DataContext _dataContext;
    private readonly IValidation _validation;

    public ForumController(
        DataContext dataContext,
        IValidation validation
    )
    {
        _dataContext = dataContext;
        _validation = validation;
    }

    private int _counter = -1;

    private int Counter
    {
        get
        {
            _counter++;
            return _counter;
        }
    }

    public IActionResult Index()
    {
        ForumIndexModel model = new()
        {
            UserCanCreate = HttpContext.User.Identity?.IsAuthenticated == true,
            Sections = _dataContext.Sections.Include(s => s.Author).OrderBy(s => s.CreatedDt).AsEnumerable()
                .Select(s =>
                {
                    return new ForumSectionModel()
                    {
                        Description = s.Description,
                        Title = s.Title,
                        Logo = $"section{Counter}.png",
                        CreatedDtString = s.CreatedDt.ToString("dd.MM.yyyy HH:mm"),
                        Author = s.Author.Username,
                        Avatar = s.Author.AvatarFileName,
                        UrlId = s.AuthorId.ToString(),
                    };
                }).ToList(),
        };
        model.IsMessagePositive = HttpContext.Session.GetInt32("IsMessagePositive") != 0;
        model.Title = HttpContext.Session.GetString("SectionTitle") ?? String.Empty;
        model.Description = HttpContext.Session.GetString("SectionDescription") ?? String.Empty;
        model.CreateMessage = HttpContext.Session.GetString("CreateMessage") ?? String.Empty;
        model.UserCanCreate = HttpContext.User.Identity?.IsAuthenticated == true;
        if (HttpContext.Session.GetString("CreateMessage") is string message)
        {
            model.CreateMessage = message;
            HttpContext.Session.Remove("CreateMessage");
        }

        return View(model);
    }

    public ViewResult Sections([FromRoute] string id)
    {
        ViewData["Id"] = id;

        var model = new ForumSectionsModel()
        {
            UserCanCreate = HttpContext.User.Identity.IsAuthenticated == true,
            Themes = _dataContext.Themes
                .OrderBy(t => t.CreatedDt).AsEnumerable().Select(t =>
                {
                    return new ForumThemeModel()
                    {
                        Description = t.Description,
                        Title = t.Title,
                        CreatedDtString = t.CreatedDt.ToString("dd.MM.yyyy HH:mm"),
                        UrlId = t.AuthorId.ToString(),
                    };
                }).ToList(),
        };
        if (HttpContext.Session.GetString("CreateMessage") is String message)
        {
            model.CreateMessage = message;
            model.IsMessagePositive =
                HttpContext.Session.GetInt32("IsMessagePositive") != 0;



            if (model.IsMessagePositive == false)
            {
                model.FormModel = new()
                {
                    Title = HttpContext.Session.GetString("SectionTitle")!,
                    Description = HttpContext.Session.GetString("SectionDescription")!
                };
                HttpContext.Session.Remove("SectionTitle");
                HttpContext.Session.Remove("SectionDescription");
            }
            

            HttpContext.Session.Remove("CreateMessage");
            HttpContext.Session.Remove("IsMessagePositive");
        }
        return View(model);
    }

    [HttpPost]
    public RedirectToActionResult CreateSection(CreateSectionModel model)
    {
        try
        {
            Console.WriteLine(model.Description);
            Console.WriteLine(model.Title);
            if (string.IsNullOrEmpty(model.Description))
            {
                HttpContext.Session.SetString("CreateMessage", "Description is empty");
                HttpContext.Session.SetInt32("IsMessagePositive", 0);
                HttpContext.Session.SetString("SectionTitle", model.Title ?? String.Empty);
                HttpContext.Session.SetString("SectionDescription", model.Description ?? String.Empty);
            }

            else if (string.IsNullOrEmpty(model.Title))
            {
                HttpContext.Session.SetString("CreateMessage", "Title is empty");
                HttpContext.Session.SetInt32("IsMessagePositive", 0);
                HttpContext.Session.SetString("SectionTitle", model.Title ?? String.Empty);
                HttpContext.Session.SetString("SectionDescription", model.Description ?? String.Empty);
            }
            else
            {
                _dataContext.Sections.Add(new()
                {
                    Title = model.Title,
                    Description = model.Description,
                    Id = Guid.NewGuid(),
                    CreatedDt = DateTime.Now,
                    AuthorId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Sid)?.Value),
                });
                _dataContext.SaveChanges();
                HttpContext.Session.SetString("CreateMessage", "Section created");
                HttpContext.Session.SetInt32("IsMessagePositive", 1);
                HttpContext.Session.SetString("SectionTitle", "");
                HttpContext.Session.SetString("SectionDescription", "");
            }
        }
        catch
        {
            HttpContext.Session.SetString("CreateMessage", "Error creating section");
            HttpContext.Session.SetInt32("IsMessagePositive", 0);
            HttpContext.Session.SetString("SectionTitle", model.Title ?? String.Empty);
            HttpContext.Session.SetString("SectionDescription", model.Description ?? String.Empty);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public RedirectToActionResult CreateTheme(CreateThemeModel model)
    {
        if (string.IsNullOrEmpty(model.Description))
        {
            HttpContext.Session.SetString("CreateMessage", "Description is empty");
            HttpContext.Session.SetInt32("IsMessagePositive", 0);
            HttpContext.Session.SetString("SectionTitle", model.Title ?? String.Empty);
            HttpContext.Session.SetString("SectionDescription", model.Description ?? String.Empty);
        }
        else if (string.IsNullOrEmpty(model.Title))
        {
            HttpContext.Session.SetString("CreateMessage", "Title is empty");
            HttpContext.Session.SetInt32("IsMessagePositive", 0);
            HttpContext.Session.SetString("SectionTitle", model.Title ?? String.Empty);
            HttpContext.Session.SetString("SectionDescription", model.Description ?? String.Empty);
        }
        else
        {
            _dataContext.Themes.Add(new()
            {
                Title = model.Title,
                Description = model.Description,
                Id = Guid.NewGuid(),
                CreatedDt = DateTime.Now,
                AuthorId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Sid)?.Value),
                SectionId = model.SectionId,
            });
            _dataContext.SaveChanges();
            HttpContext.Session.SetString("CreateMessage", "Theme created");
            HttpContext.Session.SetInt32("IsMessagePositive", 1);
            HttpContext.Session.SetString("SectionTitle", "");
            HttpContext.Session.SetString("SectionDescription", "");
        }

        return RedirectToAction(nameof(Sections), new {id = model.SectionId.ToString()});
    }
}