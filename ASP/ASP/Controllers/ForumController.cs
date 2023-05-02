using System.Security.Claims;
using ASP.Data;
using ASP.Models.Forum;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Controllers;

public class ForumController : Controller
{
    private readonly DataContext _dataContext;

    public ForumController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IActionResult Index()
    {
        ForumIndexModel model = new()
        {
            UserCanCreate = HttpContext.User.Identity?.IsAuthenticated == true,
            Sections = _dataContext.Sections.ToList(),
        };

        if (HttpContext.Session.GetString("CreateMessage") is string message)
        {
            model.CreateMessage = message;
            HttpContext.Session.Remove("CreateMessage");
        }

        return View(model);
    }

    [HttpPost]
    public RedirectToActionResult CreateSection(CreateSectionModel model)
    {
        try
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
        }
        catch
        {
        }

        return RedirectToAction(nameof(Index));
    }
}