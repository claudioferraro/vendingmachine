using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using ProductContextApi.Context;
using ProductContextApi.Controllers;
using ProductContextApi.Mappings;
using ProductContextApi.Model.DTOs;
using ProductContextApi.Model.ValueObjects;
using ProductContextApi.Repositories;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Domain.BoundedContext.Product.Model.Entities;
using VendingMachine.Domain.BoundedContext.Product.Model.Enums;

namespace VendingMachine.UnitTests
{
    public class ApiTests
    {
        private DbContextOptions<ProductContext> dbContextOptions = new DbContextOptionsBuilder<ProductContext>()
            .UseInMemoryDatabase(databaseName: "ProductDatabaseUnitTest2")
            .Options;

        private IProductRepository? _productRepository;
        private IProductMap? _productMap;
        private ProductController? _productController;
        private ProductContext _productContext;

        [SetUp]
        public void Setup()
        {
            _productContext = new ProductContext(dbContextOptions);
            _productMap = new ProductMap();
            _productRepository = new ProductRepository(_productContext, _productMap);
            _productController = new ProductController(null, _productRepository);
        }

        [Test]
        public void ProductRESTGetTest()
        {
            // cleanup
            _productContext.Database.EnsureCreated();
            _productContext.Products.RemoveRange(_productContext.Products);
            _productContext.SaveChanges();

            _productContext.Products.Add(new Product() { Name = "cola", Price  = new Money(Currency.EUR, 100) });
            _productContext.Products.Add(new Product() { Name = "chips", Price = new Money(Currency.EUR, 150) });
            _productContext.Products.Add(new Product() { Name = "candy", Price = new Money(Currency.EUR, 200) });
            _productContext.SaveChanges();
            
            // GET request
            IEnumerable<ProductDTO>? result = _productController?.Get();
            string json = JsonConvert.SerializeObject(result, Formatting.None);
            List<ProductDTO> testResult = JsonConvert.DeserializeObject<List<ProductDTO>>(json);

            Assert.IsNotNull(testResult);
            Assert.AreEqual(testResult?.Count, 3);
            Assert.AreEqual(testResult?[2].Name, "cola");
            Assert.AreEqual(testResult?[1].Price, 150);
            Assert.AreEqual(testResult?[0].Name, "candy");
        }

        [Test]
        public void ProductRESTPostTest()
        {
            List<ProductDTO> productList = new List<ProductDTO>();
            productList.AddRange(new List<ProductDTO>() 
            { 
                new ProductDTO() { Name = "hello", Currency = "EUR", Price = 199 },
                new ProductDTO() { Name = "my friend", Currency = "USD", Price = 999 } 
            });

            // POST Request
            _productController?.Create(productList);
            IEnumerable<ProductDTO>? getResult = _productController?.Get();
            string json = JsonConvert.SerializeObject(getResult, Formatting.None);
            List<ProductDTO> result = JsonConvert.DeserializeObject<List<ProductDTO>>(json);
            List<ProductDTO> testResult = new List<ProductDTO>();

            if (result is not null)
                testResult = result.Where(p => p.Name == "hello" || p.Name == "my friend").ToList();

            Assert.AreEqual(testResult?.Count, 2);
            Assert.AreEqual(testResult?[1].Name, "my friend");
            Assert.AreEqual(testResult?[0].Price, 199);
        }
    }
}