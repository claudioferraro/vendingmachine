using VendingMachine.ProductCtx.Context;
using VendingMachine.ProductCtx.Mappings;
using VendingMachine.ProductCtx.Repositories;
using VendingMachine.Domain.BoundedContext.Products.Model.Entities;
using VendingMachine.Domain.BoundedContext.Products.Model.Enums;
using VendingMachine.Domain.BoundedContext.Products.Model.ValueObjects;

var builder = WebApplication.CreateBuilder(args);

var collection = new ServiceCollection();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductStockRepository, ProductStockRepository>();
builder.Services.AddScoped<IProductMap, ProductMap>();
builder.Services.AddScoped<IProductStockMap, ProductStockMap>();
builder.Services.AddDbContext<ProductContext>();

var app = builder.Build();

// initial setup
using (var scope = app.Services.CreateScope())
{
    if (app.Environment.IsDevelopment())
    {
        // reset DB - Only for Devel
        var context = scope.ServiceProvider.GetRequiredService<ProductContext>();
        context.Database.EnsureCreated();
        context.Products.RemoveRange(context.Products);
        context.SaveChanges();

        // seed startup data
        Product prod1 =           new Product()      { Name = "cola",   Price = new Money(Currency.EUR, 1.00M) };
        ProductStock prodStock1 = new ProductStock() { Product = prod1, Stock = 15 };
        context.Products.Add(prod1);
        context.ProductStocks.Add(prodStock1);
        Product prod2 =           new Product()      { Name = "chips",  Price = new Money(Currency.EUR, 0.50M) };
        ProductStock prodStock2 = new ProductStock() { Product = prod2, Stock = 10 };
        context.Products.Add(prod2);
        context.ProductStocks.Add(prodStock2);
        Product prod3 = new Product()                { Name = "candy",  Price = new Money(Currency.EUR, 0.65M) };
        ProductStock prodStock3 = new ProductStock() { Product = prod3, Stock = 20 };
        context.Products.Add(prod3);
        context.ProductStocks.Add(prodStock3);
        // seeding ended
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
