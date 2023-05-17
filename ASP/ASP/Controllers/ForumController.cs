using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using ASP.Data;
using ASP.Data.Entity;
using ASP.Models.Forum;
using ASP.Services.Display;
using ASP.Services.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.Controllers;

public class ForumController : Controller
{
    private readonly DataContext _dataContext;
    private readonly IValidation _validation;
    private readonly IDisplayService _displayService;

    public ForumController(
        DataContext dataContext,
        IValidation validation,
        IDisplayService displayService
    )
    {
        _dataContext = dataContext;
        _validation = validation;
        _displayService = displayService;
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
            Sections = _dataContext.Sections
                .Include(s => s.Author)
                .Include(s => s.RatesList)
                .OrderBy(s => s.CreatedDt)
                .AsEnumerable()
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
                        LikeCount = s.RatesList.Count(r => r.Rating > 0),
                        DislikeCount = s.RatesList.Count(r => r.Rating < 0),
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
        ViewData["id"] = id;
        try
        {
            var model = new ForumSectionsModel()
            {
                UserCanCreate = HttpContext.User.Identity.IsAuthenticated == true,
                Themes = _dataContext.Themes.Include(t => t.Author)
                    .Where(t => t.SectionId == Guid.Parse(id))
                    .OrderBy(t => t.CreatedDt).AsEnumerable().Select(t =>
                    {
                        return new ForumThemeModel()
                        {
                            Description = t.Description,
                            Title = t.Title,
                            LogoId = t.LogoId ?? "0",
                            DaysSinceRegister =
                                _displayService.NumberOfDaysBetweenDates(DateTime.Now, t.Author.RegisterDate),
                            AuthorRegisterDateString = t.Author.RegisterDate.ToString("dd.MM.yyyy HH:mm"),
                            CreatedDtString = t.CreatedDt.ToString("dd.MM.yyyy HH:mm"),
                            UrlId = t.Id.ToString(),
                            Author = t.Author.Username,
                            Avatar = t.Author.AvatarFileName,
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
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return View();
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
    public RedirectToActionResult CreateTheme(ForumThemeFormModel model)
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
            try
            {
                _dataContext.Themes.Add(new()
                {
                    Title = model.Title,
                    Description = model.Description,
                    Id = Guid.NewGuid(),
                    LogoId = model.LogoId ?? "0",
                    CreatedDt = DateTime.Now,
                    AuthorId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Sid)?.Value),
                    SectionId = Guid.Parse(model.SectionId),
                });
                _dataContext.SaveChanges();
                HttpContext.Session.SetString("CreateMessage", "Theme created");
                HttpContext.Session.SetInt32("IsMessagePositive", 1);
                HttpContext.Session.SetString("SectionTitle", "");
                HttpContext.Session.SetString("SectionDescription", "");
            }
            catch
            {
            }
        }

        return RedirectToAction(nameof(Sections), new { id = model.SectionId.ToString() });
    }

    public IActionResult Themes([FromRoute] string id)
    {
        Theme? theme = null;
        try
        {
            theme = _dataContext.Themes.Where(t => t.Id == Guid.Parse(id)).First();
        }
        catch
        {
        }

        if (theme == null)
        {
            return NotFound();
        }

        ViewData["id"] = id;

        ForumThemesPageModel model = new()
        {
            UserCanCreate = HttpContext.User.Identity.IsAuthenticated == true,
            Title = theme.Title,
            ThemeId = id,
            CreateMessage = HttpContext.Session.GetString("CreateMessage") ?? String.Empty,
            FormModel = new ForumTopicFormModel()
            {
                Description = theme.Description,
                Title = theme.Title,
                ThemeId = theme.Id.ToString(),
            },
            Topics = _dataContext.Topics
                .Where(t => t.ThemeId == theme.Id)
                .AsEnumerable()
                .Select(t => new ForumTopicViewModel()
                {
                    Title = t.Title,
                    Author = t.Author?.Username ?? "Not found",
                    Avatar = t.Author?.Avatar ?? "no-avatar.jpg",
                    Description = _displayService.ReduceString(t.Description, 120),
                    UrlId = t.Id.ToString(),
                    CreatedDtString = _displayService.DateString(t.CreatedDt),
                }).ToList(),
            IsMessagePositive = HttpContext.Session.GetInt32("IsMessagePositive") != 0,
        };

        Console.WriteLine(model.Topics.Count);
        return View(model);
    }

    public IActionResult Topics([FromRoute] string id)
    {
        Topic? topic = null;
        try
        {
            topic = _dataContext.Topics.Where(t => t.Id == Guid.Parse(id)).First();
        }
        catch
        {
        }

        ViewData["id"] = id;
        if (topic == null)
        {
            return NotFound();
        }

        ForumTopicsPageModel model = new()
        {
            UserCanCreate = HttpContext.User.Identity?.IsAuthenticated == true,
            Title = topic.Title,
            Posts = _dataContext.Posts
                .Where(p => p.Id == topic.Id)
                .AsEnumerable()
                .Select(p => new ForumPostViewModel()
                {
                    Content = p.Content,
                    CreatedDt = _displayService.DateString(p.CreatedDt),
                    AuthorAvatar = p.Author.AvatarFileName,
                    AuthorName = p.Author.Username,
                    ReplyPreview = null
                }).ToList(),
            CreateMessage = HttpContext.Session.GetString("CreateMessage") ?? String.Empty,
            TopicId = id
        };
        return View(model);
    }

    [HttpPost]
    public RedirectToActionResult CreatePost(ForumPostFormModel model)
    {
        if (!string.IsNullOrEmpty(model.Content))
        {
            HttpContext.Session.SetString("CreateMessage", "Відповідь не може бути порожною");
            HttpContext.Session.SetInt32("IsMessagePositive", 0);
            HttpContext.Session.SetString("PostContent", model.Content ?? String.Empty);
            HttpContext.Session.SetString("PostReply", model.ReplyId ?? String.Empty);
        }
        else
        {
            Guid userId;
            try
            {
                userId = Guid.Parse(
                    HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value!
                );
                _dataContext.Posts.Add(new()
                {
                    Id = Guid.NewGuid(),
                    AuthorId = userId,
                    Content = model.Content,
                    TopicId = Guid.Parse(model.TopicId),
                    CreatedDt = DateTime.Now,
                    ReplyId = Guid.Parse(model.ReplyId)
                });
                _dataContext.SaveChanges();


                HttpContext.Session.SetString("CreateMessage", "Відповідь успішно створено");
                HttpContext.Session.SetInt32("IsMessagePositive", 1);
            }
            catch
            {
                HttpContext.Session.SetString("CreateMessage", "Помилка авторизації");
                HttpContext.Session.SetInt32("IsMessagePositive", 0);
                HttpContext.Session.SetString("PostContent", model.Content ?? String.Empty);
                HttpContext.Session.SetString("PostReply", model.ReplyId ?? String.Empty);
            }
        }

        return RedirectToAction(
            nameof(Topics),
            new { id = model.TopicId });
    }

    [HttpPost]
    public RedirectToActionResult CreateTopic(ForumTopicFormModel model)
    {
        if (string.IsNullOrEmpty(model.Description) || string.IsNullOrEmpty(model.Title))
        {
            HttpContext.Session.SetString("CreateMessage", "Description is empty");
            HttpContext.Session.SetInt32("IsMessagePositive", 0);
            HttpContext.Session.SetString("SectionTitle", model.Title ?? String.Empty);
            HttpContext.Session.SetString("SectionDescription", model.Description ?? String.Empty);
        }

        try
        {
            _dataContext.Topics.Add(new()
            {
                Description = model.Description,
                Id = Guid.NewGuid(),
                CreatedDt = DateTime.Now,
                AuthorId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Sid)?.Value),
                ThemeId = Guid.Parse(model.ThemeId),
                Title = model.Title
            });
            _dataContext.SaveChanges();
            HttpContext.Session.SetString("CreateMessage", "Theme created");
            HttpContext.Session.SetInt32("IsMessagePositive", 1);
            HttpContext.Session.SetString("SectionTitle", "");
            HttpContext.Session.SetString("SectionDescription", "");
        }
        catch
        {
        }

        return RedirectToAction(
            nameof(Themes),
            new { id = model.ThemeId });
    }
}