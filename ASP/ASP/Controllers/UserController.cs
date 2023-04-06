using ASP.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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
			else
			{
				if (!Regex.IsMatch(user.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$"))
				{
					validation.EmailMessage = "Email is not valid";
				}
			}
			if (String.IsNullOrEmpty(user.Password))
			{
				validation.PasswordMessage = "Password cant be empty";
			}
			if (String.IsNullOrEmpty(user.RepeatPassword))
			{
				validation.RepeatPasswordMessage = "Repeat password field cant be empty";
			}
			if (user.Password == user.RepeatPassword)
			{
				validation.RepeatPasswordMessage = "Passwords dont match";
				validation.PasswordMessage = "Passwords dont match";
			}
			if (user.IsAgree == false)
			{
				validation.IsAgreeMessage = "You need to agree to register";
			}
			Console.WriteLine("avatar "+user?.Avatar?.FileName);
			if (user.Avatar is not null)
			{
				var path = "wwwroot/avatars/" + user.Avatar.FileName;
				using(var fs = new FileStream(path,FileMode.Create))
				{
					user.Avatar.CopyTo(fs);
				}
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
