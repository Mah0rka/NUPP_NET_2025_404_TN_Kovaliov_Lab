using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Fish.Infrastructure
{
    // Фабрика для створення контексту під час міграцій
    public class FishContextFactory : IDesignTimeDbContextFactory<FishContext>
    {
        public FishContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FishContext>();
            
            // Connection string для PostgreSQL (для міграцій)
            optionsBuilder.UseNpgsql("Host=localhost;Database=FishDb;Username=postgres;Password=postgres");

            return new FishContext(optionsBuilder.Options);
        }
    }
}

