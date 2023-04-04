using Microsoft.AspNetCore.Mvc;

namespace ASP.Controllers
{
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}
	}
}
