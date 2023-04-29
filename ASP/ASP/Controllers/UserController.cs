using ASP.Data;
using ASP.Data.Entity;
using ASP.Models.User;
using ASP.Services.Hash;
using ASP.Services.Kdf;
using ASP.Services.Random;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Claims;
using ASP.Models;
using ASP.Models.Email;
using ASP.Services.Email;
using ASP.Services.Validation;

namespace ASP.Controllers
{
    public class UserController : Controller
    {
        private readonly IHashService _hashService;
        private readonly DataContext _dataContext;
        private IRandomService _randomService;
        private readonly ILogger<UserController> _logger;
        private readonly IKdfService _kdfService;
        private readonly IValidation _validationService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserController(
            IHashService hashService,
            DataContext dataContext,
            IRandomService randomService,
            IKdfService kdfService,
            IValidation validationService,
            IConfiguration configuration,
            IEmailService emailService,
            ILogger<UserController> logger
        )
        {
            _hashService = hashService;
            _dataContext = dataContext;
            _randomService = randomService;
            _kdfService = kdfService;
            _validationService = validationService;
            _configuration = configuration;
            _emailService = emailService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ConfirmEmail([FromBody] string code)
        {
            StatusDataModel model = new();
            // Console.WriteLine($"code {code}");
            _logger.LogInformation($"code {code}");
            _logger.LogInformation(HttpContext.User.Identity?.IsAuthenticated.ToString());
            if (String.IsNullOrEmpty(code))
            {
                model.Status = "400";
                model.Data = "Missing required param: code";
            }
            else if (HttpContext.User.Identity?.IsAuthenticated != true)
            {
                model.Status = "401";
                model.Data = "Unathenticated";
            }
            else
            {
                User? user = null;
                try
                {
                    user = _dataContext.Users.Find(Guid.Parse(
                        HttpContext.User.Claims
                            .First(claim => claim.Type == ClaimTypes.Sid).Value
                    ));
                }
                catch
                {
                }

                if (user is null)
                {
                    model.Status = "403";
                    model.Data = "Unathorized";
                }
                else if (user.EmailCode is null)
                {
                    model.Status = "208";
                    model.Data = "Already confirmed";
                }
                else if (user.EmailCode != code)
                {
                    model.Status = "406";
                    model.Data = "Code Not Accepted";
                }
                else
                {
                    user.EmailCode = null;
                    _dataContext.SaveChanges();
                    model.Status = "200";
                    model.Data = "OK";
                }
            }


            return Json(model);
        }

        public IActionResult RegisterUser(UserRegistrationModel user)
        {
            UserValidationModel validation = new();
            bool modelIsValid = true;

            if (user?.Username == null)
            {
                return View("Register");
            }

            // if (_validationService.Validate(user?.Username, ValidationTerms.NonEmpty, ValidationTerms.Username))
            // {
            //     validation.UsernameMessage = "Username cant be empty";
            //     modelIsValid = false;
            // }
            //
            // if (_validationService.Validate(user?.Email, ValidationTerms.NonEmpty))
            // {
            //     validation.EmailMessage = "Email cant be empty";
            //     modelIsValid = false;
            // }
            // else if (_dataContext.Users.Where(u => u.Email == user.Email).Any())
            // {
            //     validation.EmailMessage = "Email already in use";
            //     modelIsValid = false;
            // }
            // else
            // {
            //     if (_validationService.Validate(user.Email, ValidationTerms.Email))
            //     {
            //         validation.EmailMessage = "Email is not valid";
            //         modelIsValid = false;
            //     }
            // }
            //
            // if (_validationService.Validate(user.Password, ValidationTerms.NonEmpty))
            // {
            //     validation.PasswordMessage = "Password cant be empty";
            //     modelIsValid = false;
            // }
            //
            // if (_validationService.Validate(user.RepeatPassword, ValidationTerms.NonEmpty))
            // {
            //     validation.RepeatPasswordMessage = "Repeat password field cant be empty";
            //     modelIsValid = false;
            // }
            //
            // if (user?.Password != user?.RepeatPassword)
            // {
            //     validation.RepeatPasswordMessage = "Passwords dont match";
            //     validation.PasswordMessage = "Passwords dont match";
            //     modelIsValid = false;
            // }
            //
            // if (user?.IsAgree == false)
            // {
            //     validation.IsAgreeMessage = "You need to agree to register";
            //     modelIsValid = false;
            // }

            string avatarFileName;

            if (user?.Avatar is not null)
            {
                var extension = Path.GetExtension(user.Avatar.FileName);
                do
                {
                    string hash = _randomService.ConfirmCode(6);
                    avatarFileName = $"{hash}{extension}";
                } while (System.IO.File.Exists($"wwwroot\\avatars\\{avatarFileName}"));

                user.AvatarFileName = avatarFileName;
                var path = "wwwroot/avatars/" + avatarFileName;
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    user.Avatar.CopyTo(fs);
                }
            }

            user.EmailCode = _randomService.ConfirmCode(6);


            if (modelIsValid)
            {
                string salt = _randomService.Random(8);
                var DBuser = new User()
                {
                    Id = Guid.NewGuid(),
                    Avatar = user.AvatarFileName,
                    Email = user.Email,
                    Username = user.Username,
                    EmailCode = _randomService.ConfirmCode(6),
                    LastLogin = DateTime.Now,
                    PasswordHash = _kdfService.GetDerivedKey(user.Password, salt),
                    PasswordSalt = salt,
                    RegisterDate = DateTime.Now,
                    AvatarFileName = user.AvatarFileName,
                };
                _dataContext.Users.Add(DBuser);

                EmailConfirmationToken emailConfirmToken = _GenerateConfirmToken(DBuser);
                _SendEmail(DBuser, emailConfirmToken);
                _dataContext.SaveChanges();
            }

            ViewData["user"] = user;
            ViewData["validation"] = validation;

            return View("Register");
        }

        private void _SendEmail(User user, EmailConfirmationToken token)
        {
            String confirmUrl =
                $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/User/ConfirmToken?token={token.Id}";

            Console.WriteLine("Sending email...");
            _emailService.Send("ConfirmEmail", new ConfirmEmailModel()
            {
                Email = user.Email,
                EmailCode = user.EmailCode,
                Username = user.Username,
                ConfirmURL = confirmUrl,
            });
        }

        private EmailConfirmationToken _GenerateConfirmToken(Data.Entity.User user)
        {
            Data.Entity.EmailConfirmationToken emailToken = new()
            {
                Id = Guid.NewGuid(),
                Moment = DateTime.Now,
                UserEmail = user.Email,
                UserId = user.Id,
                Used = 0
            };
            _dataContext.EmailToken.Add(emailToken);
            return emailToken;
        }

        [HttpPatch]
        public string ResendEmailConfirmCode()
        {
            if (HttpContext.User.Identity?.IsAuthenticated != true)
            {
                return "Unauthenticated";
            }

            User user = null;
            try
            {
                user = _dataContext.Users.Find(Guid.Parse(HttpContext.User.Claims
                    .First(claim => claim.Type == ClaimTypes.Sid).Value));
            }
            catch
            {
            }

            if (user is null)
            {
                return "Unauthorized";
            }

            EmailConfirmationToken emailConfirmToken = _GenerateConfirmToken(user);
            _SendEmail(user, emailConfirmToken);
            _dataContext.SaveChanges();
            return "OK";
        }

        [HttpPost]
        public string Login()
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
                Console.WriteLine(user.Id.ToString());
                HttpContext.Session.SetString("userId", user.Id.ToString());
                return "OK";
            }
            else
            {
                return "Wrong login or password";
            }
        }

        public ViewResult ConfirmToken([FromQuery] string token)
        {
            ViewData["token"] = token;
            Guid tokenId;
            try
            {
                tokenId = Guid.Parse(token);
            }
            catch
            {
                ViewData["token"] = "Invalid token plaese dont change link in email";
                return View();
            }

            var emailConfirmToken = _dataContext.EmailToken.FirstOrDefault(t => t.Id == tokenId);

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userId");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Profile([FromRoute] String id)
        {
            try
            {
                Data.Entity.User? user = _dataContext.Users.FirstOrDefault(u => u.Id == Guid.Parse(id));
                if (user is not null)
                {
                    Models.User.ProfileModel model = new(user);
                    if (string.IsNullOrEmpty(model.Avatar))
                    {
                        model.Avatar = string.IsNullOrEmpty(user.Avatar) ? user.Avatar : "no-avatar.png";
                    }

                    if (HttpContext.User.Identity is not null
                        && HttpContext.User.Identity.IsAuthenticated)
                    {
                        string userLogin = HttpContext.User.Claims
                            .First(claim => claim.Type == ClaimTypes.NameIdentifier)
                            .Value;

                        if (model.Login == userLogin)
                        {
                            model.IsPersonal = true;
                        }
                    }

                    return View(model);
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return NotFound();
            }
        }
    }
}