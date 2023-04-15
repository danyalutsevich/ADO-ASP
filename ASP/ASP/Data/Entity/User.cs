namespace ASP.Data.Entity
{
	public class User
	{
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string EmailCode { get; set; }
		public string PasswordHash { get; set; }
		public string PasswordSalt { get; set; }
		public string? Avatar { get; set; }
		public DateTime RegisterDate { get; set; }
		public DateTime? LastLogin { get; set; }
		public string? AvatarFileName { get; set; }


	}
}
