using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using VendingMachine.Domain.BoundedContext.Products.Model.ValueObjects;
using VendingMachine.Domain.BoundedContext.Products.Model.Entities;

namespace VendingMachine.ProductCtx.Context.Fluent
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        /* legenda: p = product, ps = productstock, v = value */
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable ("Product");

            builder.HasKey  (product => product.Id);

            builder.Property(product => product.Name)
                   .HasColumnName("Name");

            builder.Property(product => product.Price)
                   .HasConversion(
                       v => JsonConvert.SerializeObject(v, Formatting.Indented),
                       v => JsonConvert.DeserializeObject<Money>(v));

            builder.HasOne<ProductStock>(p => p.Stock)
                   .WithOne(ps => ps.Product)
                   .HasForeignKey<ProductStock>(ps => ps.ProductStockId);
        }
    }
}
