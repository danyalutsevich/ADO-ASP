using ASP.Data.Entity;
using ASP.Models.Email;
using Microsoft.EntityFrameworkCore;

namespace ASP.Data
{
	public class DataContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<EmailConfirmationToken> EmailToken { get; set; }

		public DataContext(DbContextOptions options) : base(options)
		{

		}

	}
}
