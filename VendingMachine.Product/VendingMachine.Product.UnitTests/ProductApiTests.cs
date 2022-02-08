using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using VendingMachine.ProductCtx.Context;
using VendingMachine.ProductCtx.Controllers;
using VendingMachine.ProductCtx.Mappings;
using VendingMachine.ProductCtx.Model.DTOs;
using VendingMachine.ProductCtx.Repositories;
using VendingMachine.Domain.BoundedContext.Products.Model.Enums;
using VendingMachine.Domain.BoundedContext.Products.Model.Entities;
using VendingMachine.Domain.BoundedContext.Products.Model.ValueObjects;

using System.Collections.Generic;
using System.Linq;

namespace VendingMachine.Product.UnitTests
{
    public class ProductApiTests
    {
        private DbContextOptions<ProductContext> dbContextOptions = new DbContextOptionsBuilder<ProductContext>()
            .UseInMemoryDatabase(databaseName: "ProductDatabaseUnitTest2")
            .Options;

        private ProductContext     _productContext;
        private IProductMap        _productMap;
        private IProductRepository _productRepository;
        private ProductController  _productController;

        [SetUp]
        public void Setup()
        {
            _productContext    = new ProductContext(dbContextOptions);
            _productMap        = new ProductMap();
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

            _productContext.Products.Add(new Domain.BoundedContext.Products.Model.Entities.Product() { Name = "cola",  Price = new Money(Currency.EUR, 100) });
            _productContext.Products.Add(new Domain.BoundedContext.Products.Model.Entities.Product() { Name = "chips", Price = new Money(Currency.EUR, 150) });
            _productContext.Products.Add(new Domain.BoundedContext.Products.Model.Entities.Product() { Name = "candy", Price = new Money(Currency.EUR, 200) });
            _productContext.SaveChanges();
            
            // GET request
            IEnumerable<ProductDTO> productDTOs = _productController.Get();
            string json                  = JsonConvert.SerializeObject(productDTOs, Formatting.None);
            List<ProductDTO> productList = JsonConvert.DeserializeObject<List<ProductDTO>>(json);

            Assert.IsNotNull(productList);
            Assert.IsTrue(productList.Count >= 3);
            Assert.AreEqual(productList?[2].Name,  "cola");
            Assert.AreEqual(productList?[1].Price, 150);
            Assert.AreEqual(productList?[0].Name,  "candy");
        }

        [Test]
        /* We POST and GET, serialize and deserialize and check the result */
        public void ProductRESTPostTest()
        {
            List<ProductDTO> productList = new List<ProductDTO>();
            ProductDTO productDTO        = new ProductDTO() { Name = "hello", Currency = "EUR", Price = 199 };

            // POST Request
            _productController.Create(productDTO);
            IEnumerable<ProductDTO> productDTOs = _productController.Get();
            string json = JsonConvert.SerializeObject(productDTOs, Formatting.None);
            productList = JsonConvert.DeserializeObject<List<ProductDTO>>(json);

            if (productList is not null)
                productList = productList.Where(p => p.Name == "hello" || p.Name == "my friend").ToList();

            Assert.IsNotNull(productList);
            Assert.AreEqual(productList?.Count,   1);
            Assert.AreEqual(productList?[0].Name, "hello");
            Assert.AreEqual(productList?[0].Price, new decimal(199));
        }
    }
}