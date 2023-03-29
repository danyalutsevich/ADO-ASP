using ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Diagnostics;

namespace ASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Intro()
        {
            return View();
        }
        
        public IActionResult Scheme()
        {
			ViewBag.Message = "Text from bag";
			return View();
        }
        
        public IActionResult AboutURL()
        {
			ViewBag.Query = Request.Query;
			ViewBag.Path = Request.Path;
			ViewBag.Request = Request;
			return View();
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}