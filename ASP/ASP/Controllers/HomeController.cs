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

		public IActionResult Table()
		{
			var products = new List<Product>()
			{
				new Product{Name="Apple", Price=10000000.55f},
				new Product{Name="Banana", Price=35000000.87f},
				new Product{Name="Orange", Price=21000000.23f},
				new Product{Name="Bradley", Price=5000000.42f},
				new Product{Name="Chinook", Price=38000000.91f},
				new Product{Name="F16", Price=18000000.67f},
				new Product{Name="F35", Price=95000000.11f},
				new Product{Name="Grapes", Price=220000.75f},
				new Product{Name="M16", Price=750.36f},
				new Product{Name="M1A1", Price=8000000.24f},
				new Product{Name="Pineapple", Price=12000.68f},
				new Product{Name="M249", Price=7600.92f},
				new Product{Name="M2", Price=106000.13f},
				new Product{Name="Strawberry", Price=1300.47f},
				new Product{Name="M9", Price=600.78f},
				new Product{Name="Kiwi", Price=350000.61f},
				new Product{Name="Patriot", Price=5000000.17f},
				new Product{Name="Stryker", Price=5000000.39f},
				new Product{Name="Watermelon", Price=25000.99f},
				new Product{Name="Leopard 2", Price=12000000.73f},
			};



			ViewBag.Products = products;

			return View();
		}

		public IActionResult Razor()
		{
			return View();
		}

		public IActionResult Model()
		{
			var model = new Models.Home.Model()
			{
				Header = "Header from Model",
				Title = "Title from Model",

				Departments = new List<string>() { "IT", "HR", "Sales", "Managers" },
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