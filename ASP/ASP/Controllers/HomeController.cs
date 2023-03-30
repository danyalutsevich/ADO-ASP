using ASP.Models;
using ASP.Models.Home;
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

        public IActionResult Razor()
        {
            return View();
        }

        public IActionResult Model()
        {
            var model = new Models.Home.Model() { 
                Header = "Header from Model",
                Title = "Title from Model",

				Departments = new List<string>() { "IT", "HR", "Sales","Managers" },
                Products = new List<Product>()
                {
                    new Product{Name="Pasta",Price=3},
					new Product{Name="Bread",Price=2},
                    new Product{Name="Juice",Price=1}
				}
             
			};
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}