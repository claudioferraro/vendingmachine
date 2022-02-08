using VendingMachine.ProductCtx.Context;
using VendingMachine.ProductCtx.Mappings;
using VendingMachine.ProductCtx.Model.DTOs;
using VendingMachine.Domain.BoundedContext.Products.Model.Entities;

namespace VendingMachine.ProductCtx.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private ProductContext _pc;

        private readonly ProductMap _productMap;

        public ProductRepository(ProductContext productContext, IProductMap productMap)
        {
            _pc = productContext;
            _productMap = (ProductMap)productMap;
        }

        public void AddItem(ProductDTO item)
        {
            _pc.Products.Add(_productMap.Map(item));
        }

        public void DeleteItem(int id)
        {
            Product product = new Product() { Id = id };
            _pc.Products.Attach(product);
            _pc.Products.Remove(product);
        }

        public IEnumerable<ProductDTO> FetchAll()
        {
            return _pc.Products.Select(
                entity => _productMap.InverseMap(entity));
        }

        public void SubmitChanges()
        {
            _pc.SaveChanges();
        }
    }
}
