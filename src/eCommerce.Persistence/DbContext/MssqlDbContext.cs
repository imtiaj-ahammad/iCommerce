using eCommerce.Domain;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Persistence;

public class MssqlDbContext  : DbContext
{
    public MssqlDbContext(DbContextOptions<MssqlDbContext> options) : base(options) { }

		public DbSet<ProductCategory> ProductCategories { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<Actor>().HasData(
					new Actor { Id = 1, FirstName = "Chuck", LastName = "Norris" }
					, new Actor { Id = 2, FirstName = "Jane", LastName = "Doe" }
					, new Actor { Id = 3, FirstName = "Van", LastName = "Damme" }
				);
        }*/
}
