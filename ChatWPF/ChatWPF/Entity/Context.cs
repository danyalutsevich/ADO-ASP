using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace ChatWPF.Entity
{
	public class Context : DbContext
	{
		public DbSet<Chat> Chats { get; set; }
		public DbSet<Message> Messages { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			DotNetEnv.Env.Load();
			var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING_EFCORE");
			//optionsBuilder.UseSqlServer(connectionString);
			optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Chat>()
				.HasMany(c => c.Messages)
				.WithOne(m => m.Chat)
				.HasForeignKey(m=>m.ChatId)
				.HasPrincipalKey(c => c.Id);
		}
	}
}
