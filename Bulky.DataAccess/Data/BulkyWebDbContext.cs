using BulkyBook.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Data
{
    public class BulkyBookWebDbContext : DbContext
    {
        public BulkyBookWebDbContext(DbContextOptions<BulkyBookWebDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category() { CategoryId = 1, Name = "Action", DisplayOrder = 1 },
                new Category() { CategoryId = 2, Name = "Sci-Fi", DisplayOrder = 2 },
                new Category() { CategoryId = 3, Name = "Periodic Drama", DisplayOrder = 3 },
                new Category() { CategoryId = 4, Name = "Comedy", DisplayOrder = 4 },
                new Category() { CategoryId = 5, Name = "Horror", DisplayOrder = 5 });
        }
    }
}
