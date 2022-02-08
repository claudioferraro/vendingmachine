using Microsoft.AspNetCore.Mvc;
using VendingMachine.ProductCtx.Repositories;
using VendingMachine.ProductCtx.Model.DTOs;

namespace VendingMachine.ProductCtx.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductRepository _productRepository;

        public ProductController(ILogger<ProductController> logger, IProductRepository productRepository)
        {
            _logger            = logger;
            _productRepository = productRepository;
        }

        [HttpGet(Name = "GetProduct")]
        public IEnumerable<ProductDTO> Get() => _productRepository.FetchAll();

        [HttpPost]
        public ActionResult Create(ProductDTO productDTO)
        {
            try
            {
                _productRepository.AddItem(productDTO);
                _productRepository.SubmitChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }
    }
}