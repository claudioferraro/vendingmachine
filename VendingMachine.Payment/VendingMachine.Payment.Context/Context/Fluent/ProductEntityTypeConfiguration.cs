using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using ProductContextApi.Model.ValueObjects;
using VendingMachine.Domain.BoundedContext.Product.Model.Entities;

namespace ProductContextApi.Context.Fluent
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(product => product.Id);

            builder.Property(product => product.Name)
                   .HasColumnName("Name");

            builder.Property(product => product.Price)
                   .HasConversion(
                       v => JsonConvert.SerializeObject(v, Formatting.Indented),
                       v => JsonConvert.DeserializeObject<Money>(v));
        }
    }
}
