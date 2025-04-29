using Microsoft.EntityFrameworkCore;
using PsxDataHelper.Application.Entity.Game;

namespace PsxDataHelper.Application
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
