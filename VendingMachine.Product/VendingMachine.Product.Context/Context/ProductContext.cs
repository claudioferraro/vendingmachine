using Microsoft.EntityFrameworkCore;
using VendingMachine.ProductCtx.Context.Fluent;
using VendingMachine.Domain.BoundedContext.Products.Model.Entities;
using VendingMachine.Domain.BoundedContext.Products.Model.Enums;
using VendingMachine.Domain.BoundedContext.Products.Model.ValueObjects;

namespace VendingMachine.ProductCtx.Context
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }
        public DbSet<Product>      Products { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());

            // seeding for Migrations - Stocks currently not supported!
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
