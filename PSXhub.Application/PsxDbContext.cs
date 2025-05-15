using Microsoft.EntityFrameworkCore;
using PSXhub.Application.Entity.Game;

namespace PSXhub.Application
{
	public class PsxDbContext : DbContext
	{
		public DbSet<GamePath> GamePaths { get; set; }
		public PsxDbContext()
		{
			Database.MigrateAsync().Wait();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			optionsBuilder.UseSqlite(@"Data Source=db.db");
		}
	}
}
