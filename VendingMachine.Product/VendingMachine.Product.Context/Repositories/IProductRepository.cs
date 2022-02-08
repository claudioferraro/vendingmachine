using VendingMachine.ProductCtx.Model.DTOs;
using VendingMachine.ProductCtx.Repositories;

namespace VendingMachine.ProductCtx.Repositories
{
    public interface IProductRepository : IRepository<ProductDTO> {}
}
