using ASP.Data.Entity;
using ASP.Models.Email;
using Microsoft.EntityFrameworkCore;

namespace ASP.Data
{
	public class DataContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<EmailConfirmationToken> EmailToken { get; set; }

		public DbSet<Section> Sections { get; set; }
		public DbSet<Theme> Themes { get; set; }
		public DbSet<Topic> Topics { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Rate> Rates { get; set; }
		
		public DataContext(DbContextOptions options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Rate>()
				.HasKey(nameof(Rate.ItemId), nameof(Rate.UserId));
			
			modelBuilder.Entity<Entity.Section>()
				.HasOne(s => s.Author)
				.WithMany()
				.HasForeignKey(s => s.AuthorId);
		}
		
	}
}
