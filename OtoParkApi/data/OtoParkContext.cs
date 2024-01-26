// AutoParkContext.cs

using Microsoft.EntityFrameworkCore;

namespace OtoParkApi
{
    public class OtoParkContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Pricing> Pricings { get; set; }

        public OtoParkContext(DbContextOptions<OtoParkContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationships or constraints if needed
            // Example:
            // modelBuilder.Entity<Car>().HasKey(c => c.Id);
            // modelBuilder.Entity<Pricing>().HasKey(p => p.Id);

            // Seed default pricing data
            modelBuilder.Entity<Pricing>().HasData(
                new Pricing { Id = 1, MinHours = 0, MaxHours = 1, Price = 50 },
                new Pricing { Id = 2, MinHours = 1, MaxHours = 12, Price = 75 },
                new Pricing { Id = 3, MinHours = 12, MaxHours = 24, Price = 100 }
            );
        }
    }
}
