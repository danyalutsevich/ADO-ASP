using ASP.Data;
using ASP.Data.Entity;
using ASP.Models.User;
using ASP.Services.Hash;
using ASP.Services.Kdf;
using ASP.Services.Random;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.IO;

namespace ASP.Controllers
{
	public class UserController : Controller
	{
		private readonly IHashService _hashService;
		private readonly DataContext _dataContext;
		private IRandomService _randomService;
		private readonly IKdfService _kdfService;

		public UserController(IHashService hashService, DataContext dataContext, IRandomService randomService, IKdfService kdfService)
		{
			_hashService = hashService;
			_dataContext = dataContext;
			_randomService = randomService;
			_kdfService = kdfService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult RegisterUser(UserRegistrationModel user)
		{
			UserValidationModel validation = new();
			bool modelIsValid = true;

			if (String.IsNullOrEmpty(user?.Username))
			{
				validation.UsernameMessage = "Username cant be empty";
				modelIsValid = false;
			}
			if (String.IsNullOrEmpty(user?.Email))
			{
				validation.EmailMessage = "Email cant be empty";
				modelIsValid = false;
			}
			else
			{
				if (!Regex.IsMatch(user?.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$"))
				{
					validation.EmailMessage = "Email is not valid";
					modelIsValid = false;
				}
			}
			if (String.IsNullOrEmpty(user?.Password))
			{
				validation.PasswordMessage = "Password cant be empty";
				modelIsValid = false;
			}
			if (String.IsNullOrEmpty(user?.RepeatPassword))
			{
				validation.RepeatPasswordMessage = "Repeat password field cant be empty";
				modelIsValid = false;
			}
			if (user?.Password != user?.RepeatPassword)
			{
				validation.RepeatPasswordMessage = "Passwords dont match";
				validation.PasswordMessage = "Passwords dont match";
				modelIsValid = false;
			}
			if (user?.IsAgree == false)
			{
				validation.IsAgreeMessage = "You need to agree to register";
				modelIsValid = false;
			}

			string avatarFileName;

			if (user?.Avatar is not null)
			{
				var extension = Path.GetExtension(user.Avatar.FileName);
				do
				{
					string hash = _hashService.Hash(user.Avatar.FileName + Guid.NewGuid())[..16];
					avatarFileName = $"{hash}{extension}";
				} while (System.IO.File.Exists($"wwwroot\\avatars\\{avatarFileName}"));

				user.AvatarFileName = avatarFileName;
				var path = "wwwroot/avatars/" + avatarFileName;
				using (var fs = new FileStream(path, FileMode.Create))
				{
					user.Avatar.CopyTo(fs);
				}
			}
			
			user.EmailCode = Guid.NewGuid().ToString()[..6];


			if (modelIsValid)
			{
				String salt = _randomService.Random(8);
				var DBuser = new User()
				{
					Id = Guid.NewGuid(),
					Avatar = null,
					Email = user.Email,
					Username = user.Username,
					EmailCode = _randomService.ConfirmCode(6),
					LastLogin = DateTime.Now,
					PasswordHash = _kdfService.GetDerivedKey(user.Password, salt),
					PasswordSalt = salt,
					RegisterDate = DateTime.Now
				};
				_dataContext.Users.Add(DBuser);
				_dataContext.SaveChanges();
			}

			ViewData["user"] = user;
			ViewData["validation"] = validation;

			return View("Register");
		}

		[HttpPost]
		public string Register()
		{
			var loginValues = Request.Form["user-login"];
			if (loginValues.Count == 0)
			{
				return "No login data";
			}
			var login = loginValues[0];

			var passwordValues = Request.Form["user-password"];

			if (passwordValues.Count == 0)
			{
				return "No password data";
			}
			var password = passwordValues[0];


			var user = _dataContext.Users.FirstOrDefault(u => u.Email == login);

			if (user is not null && user.PasswordHash == _kdfService.GetDerivedKey(password, user.PasswordSalt))
			{
				return "Logged in";
			}
			else
			{
				return "Wrong login or password";
			}
		}
	}
}
