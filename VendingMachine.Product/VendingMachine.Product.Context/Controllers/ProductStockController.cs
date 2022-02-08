using Microsoft.AspNetCore.Mvc;
using VendingMachine.ProductCtx.Repositories;
using VendingMachine.ProductCtx.Model.DTOs;

namespace VendingMachine.ProductCtx.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductStockController : ControllerBase
    {
        private readonly ILogger<ProductStockController>        _logger;

        private readonly ProductStockRepository _productStockRepository;

        public ProductStockController(ILogger<ProductStockController> logger, IProductStockRepository productStockRepository)
        {
            _productStockRepository = (ProductStockRepository)productStockRepository;
            _logger                 = logger;
        }

        [HttpGet(Name = "GetProductStock")]
        public IEnumerable<ProductStockDTO> Get() => _productStockRepository.FetchAll();

        [HttpPut("{id:int}")]
        public ActionResult UpdateStock(int id, ProductStockDTO productStock)
        {
            try
            {
                if (id != productStock.ProductId)
                    return BadRequest("Product ID mismatch");

                _productStockRepository.UpdateStockByProductId(id);
                _productStockRepository.SubmitChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    String.Format("Error updating Stock for Id {0}", ex.Message));
            }
        }

        [HttpPost]
        public ActionResult Create(ProductStockDTO productDTO)
        {
            try
            {
                _productStockRepository.AddItem(productDTO);
                _productStockRepository.SubmitChanges();
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