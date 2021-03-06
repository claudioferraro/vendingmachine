using VendingMachine.Domain.BoundedContext.Products.Model.Entities;
using VendingMachine.ProductCtx.Model.DTOs;

namespace VendingMachine.ProductCtx.Mappings
{
    public interface IProductMap : IMap<ProductDTO, Product> {}

}
