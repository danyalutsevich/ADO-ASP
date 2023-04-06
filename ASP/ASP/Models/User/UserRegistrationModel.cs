namespace ASP.Models.User
{
    public class UserRegistrationModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public bool IsAgree { get; set; }
        public IFormFile Avatar { get; set; }
    }
}
