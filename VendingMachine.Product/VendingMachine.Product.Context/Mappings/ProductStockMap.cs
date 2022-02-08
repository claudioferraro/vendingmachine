using VendingMachine.ProductCtx.Model.DTOs;
using VendingMachine.Domain.BoundedContext.Products.Model.Entities;
using VendingMachine.ProductCtx.Context;

namespace VendingMachine.ProductCtx.Mappings
{
    public class ProductStockMap : IProductStockMap
    {
        private readonly ProductContext _productContext;
        public ProductStockMap(ProductContext productContext) { _productContext = productContext; }

        public ProductStock Map(ProductStockDTO dto)
        {
            var product = _productContext.Products.Find(dto.ProductId);
            return new ProductStock()
            {
                ProductStockId = dto.ProductStockId,
                Stock          = dto.Stock,
                Product        = product
            };
        }

        public ProductStockDTO InverseMap(ProductStock entity)
        {
            return new ProductStockDTO()
            {
                ProductStockId = entity.ProductStockId,
                Stock          = entity.Stock,
                ProductId      = entity.Product.Id
            };
        }
    }
}
