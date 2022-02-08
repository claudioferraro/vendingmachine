using ProductContextApi.Context;
using ProductContextApi.Mappings;
using ProductContextApi.Model.ValueObjects;
using ProductContextApi.Repositories;
using VendingMachine.Domain.BoundedContext.Product.Model.Entities;
using VendingMachine.Domain.BoundedContext.Product.Model.Enums;

var builder = WebApplication.CreateBuilder(args);

var collection = new ServiceCollection();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductMap, ProductMap>();
builder.Services.AddDbContext<ProductContext>();
var app = builder.Build();

// initial setup
using (var scope = app.Services.CreateScope())
{
    // reset DB
    var context = scope.ServiceProvider.GetRequiredService<ProductContext>();
    context.Database.EnsureCreated();
    context.Products.RemoveRange(context.Products);
    context.SaveChanges();

    // seed startup data
    context.Products.Add(new Product() { Id = 1, Name = "cola", Price = new Money(Currency.EUR, 100) });
    context.Products.Add(new Product() { Id = 2, Name = "chips", Price = new Money(Currency.EUR, 150) });
    context.Products.Add(new Product() { Id = 3, Name = "candy", Price = new Money(Currency.EUR, 200) });
    context.SaveChanges();
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
