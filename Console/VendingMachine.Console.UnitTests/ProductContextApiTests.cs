using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Console.Model.DTOs;
using VendingMachine.Console.Services.ProductContext;
using VendingMachine.Console.WebClient;

namespace VendingMachine.Console.UnitTests
{
    public class ProductContextApiTests
    {
        private IConfigurationRoot           _configuration;
        private IProductService             _productService;
        private IProductStockService   _productStockService;

        private IHttpRESTClient<ProductDTO>           _httpRESTProductClient;
        private IHttpRESTClient<ProductStockDTO> _httpRESTProductStockClient;

        [SetUp]
        public void Setup()
        {
            var appSettings = 
            @"{
                ""Protocol"" : ""https"", 
                ""Host""     : ""localhost"", 
                ""Url""      : """",
                ""Product""  : {
                    ""Port""     : 7287, 
                    ""EndPoint"" : ""Product""
                }
            }";

            var builder = new ConfigurationBuilder()
                .AddJsonStream(
                    new MemoryStream(Encoding.UTF8.GetBytes(appSettings))
                 );

            _configuration              = builder.Build();
            _httpRESTProductClient      = new HttpRESTClient<ProductDTO>(_configuration);
            _httpRESTProductStockClient = new HttpRESTClient<ProductStockDTO>(_configuration);
            _productService             = new ProductService(_configuration, _httpRESTProductClient);
            _productStockService        = new ProductStockService(_configuration, _httpRESTProductStockClient);
        }

        [Test]
        public async Task ProductRestGetAllTest()
        {
            IEnumerable<ProductDTO> productDTOs = await _productService.GetAllAsync();
            List<ProductDTO> productList = productDTOs.ToList();

            Assert.IsNotNull(productList);
            Assert.AreEqual (productList?[2].Name, "cola");
            Assert.AreEqual (productList?[1].Price, 1.50M);
            Assert.AreEqual (productList?[0].Name, "candy");
            Assert.IsTrue   (productList.Count >= 3);
        }

        [Test]
        public async Task ProductStockRestGetAllTest()
        {
            IEnumerable<ProductStockDTO> productStockDTOs = await _productStockService.GetAllAsync();
            List<ProductStockDTO> productStockList = productStockDTOs.ToList();

            Assert.IsNotNull(productStockList);
            Assert.AreEqual (productStockList?[2].Stock, 20);
            Assert.AreEqual (productStockList?[1].Stock, 10);
            Assert.AreEqual (productStockList?[0].Stock, 2);
            Assert.IsTrue   (productStockList.Count >= 3);
        }

        [Test]
        /* We POST and GET, serialize and deserialize and check the result */
        public async Task ProductRestPOSTTest()
        {
            List<ProductDTO> productList = new List<ProductDTO>();
            ProductDTO productDTOPOST    = new ProductDTO() { Name = "hello", Currency = "EUR", Price = 1.99M };
            ProductDTO productDTO        = await _productService.PostAsync(productDTOPOST);

            IEnumerable<ProductDTO> productDTOs = await _productService.GetAllAsync();
            string json = JsonConvert.SerializeObject(productDTOs, Formatting.None);
            productList = JsonConvert.DeserializeObject<List<ProductDTO>>(json);

            if (productList is not null)
                productList = productList.Where(p => p.Name == "hello").ToList();
            Assert.IsNotNull(productList);
            Assert.AreEqual(productList?[0].Name,     "hello");
            Assert.AreEqual(productList?[0].Currency, "EUR");
            Assert.AreEqual(productList?[0].Price, 1.99M);
        }
    }
}