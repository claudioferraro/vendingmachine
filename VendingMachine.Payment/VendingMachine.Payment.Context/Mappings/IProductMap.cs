using ProductContextApi.Model.DTOs;
using VendingMachine.Domain.BoundedContext.Product.Model.Entities;

namespace ProductContextApi.Mappings
{
    public interface IProductMap : IMap<ProductDTO, Product>
    {}
}
