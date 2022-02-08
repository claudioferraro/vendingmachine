using Microsoft.EntityFrameworkCore;
using VendingMachine.Domain.BoundedContext.Product.Model.Entities;
using VendingMachine.Domain.BoundedContext.Product.Model.Enums;
using ProductContextApi.Context.Fluent;
using ProductContextApi.Model.ValueObjects;

namespace ProductContextApi.Context
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());

            // seeding for Migrations
            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "cola",  Price = new Money(Currency.EUR, 100) },
                new Product() { Id = 2, Name = "chips", Price = new Money(Currency.EUR, 150) },
                new Product() { Id = 3, Name = "candy", Price = new Money(Currency.EUR, 200) }
            );
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "ProductDatabase");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
