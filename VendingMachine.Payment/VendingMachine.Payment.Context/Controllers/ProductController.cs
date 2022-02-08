using Microsoft.AspNetCore.Mvc;
using ProductContextApi.Model.DTOs;
using ProductContextApi.Repositories;

namespace ProductContextApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController>? _logger;

        private readonly ProductRepository _productRepository;

        public ProductController(ILogger<ProductController>? logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = (ProductRepository)productRepository;
        }

        [HttpGet(Name = "GetProduct")]
        public IEnumerable<ProductDTO> Get() => _productRepository.FetchAll();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IEnumerable<ProductDTO> productCollection)
        {
            try
            {
                foreach (var product in productCollection) 
                    _productRepository.AddItem(product);
                _productRepository.SubmitChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                return StatusCode(500);
            }
        }
    }
}