using ASP.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Controllers
{
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult RegisterUser(UserRegistrationModel user)
		{
			UserValidationModel validation = new();

			if (String.IsNullOrEmpty(user.Username))
			{
				validation.UsernameMessage = "Username cant be empty";
			}
			if (String.IsNullOrEmpty(user.Email))
			{
				validation.EmailMessage = "Email cant be empty";
			}
			if (String.IsNullOrEmpty(user.Password))
			{
				validation.PasswordMessage = "Password cant be empty";
			}
			if (String.IsNullOrEmpty(user.RepeatPassword))
			{
				validation.RepeatPasswordMessage = "Repeat password field cant be empty";
			}
			if (user.IsAgree == false)
			{
				validation.IsAgreeMessage = "You need to agree to register";
			}


			ViewData["user"] = user;
			ViewData["validation"] = validation;
			return View("Register");
		}

		public IActionResult Register()
		{
			return View();
		}
	}
}
