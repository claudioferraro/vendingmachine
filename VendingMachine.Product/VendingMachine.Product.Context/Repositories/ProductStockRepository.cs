using VendingMachine.ProductCtx.Context;
using VendingMachine.ProductCtx.Mappings;
using VendingMachine.ProductCtx.Model.DTOs;
using VendingMachine.Domain.BoundedContext.Products.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace VendingMachine.ProductCtx.Repositories
{
    public class ProductStockRepository : IProductStockRepository
    {
        private ProductContext _pc;

        private readonly ProductStockMap _productStockMap;

        public ProductStockRepository(ProductContext productContext, IProductStockMap productstockMap)
        {
            _pc              = productContext;
            _productStockMap = (ProductStockMap)productstockMap;
        }

        public void AddItem(ProductStockDTO item)
        {
            _pc.ProductStocks.Add(_productStockMap.Map(item));
        }

        public void DeleteItem(int id)
        {
            ProductStock productStock = new ProductStock() { ProductStockId = id };
            _pc.ProductStocks.Attach(productStock);
            _pc.ProductStocks.Remove(productStock);
        }

        public IEnumerable<ProductStockDTO> FetchAll()
        {
            return _pc.ProductStocks.Include(x => x.Product).Select(
                entity => _productStockMap.InverseMap(entity));
        }

        public void UpdateStockByProductId(int id)
        {
            var productStock = _pc.ProductStocks.SingleOrDefault(ps => ps.Product.Id == id);
            if (productStock != null)
            {
                productStock.Stock--;
            }
        }

        public void SubmitChanges()
        {
            _pc.SaveChanges();
        }
    }
}
