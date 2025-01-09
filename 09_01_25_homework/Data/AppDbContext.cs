using _09_01_25_homework.Model;
using Microsoft.EntityFrameworkCore;

namespace _09_01_25_homework.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
         

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Задаём данные по умолчанию для таблицы Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Product 1", Price = 10.99m },
                new Product { Id = 2, Name = "Product 2", Price = 15.49m },
                new Product { Id = 3, Name = "Product 3", Price = 25.00m },
                new Product { Id = 4, Name = "Product 4", Price = 5.00m },
                new Product { Id = 5, Name = "Product 5", Price = 50.75m },
                new Product { Id = 6, Name = "Product 6", Price = 12.30m },
                new Product { Id = 7, Name = "Product 7", Price = 30.00m },
                new Product { Id = 8, Name = "Product 8", Price = 18.20m },
                new Product { Id = 9, Name = "Product 9", Price = 22.15m },
                new Product { Id = 10, Name = "Product 10", Price = 99.99m }
            );
        }
    }
}
