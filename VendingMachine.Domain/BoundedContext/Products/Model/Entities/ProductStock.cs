namespace VendingMachine.Domain.BoundedContext.Products.Model.Entities
{
    public class ProductStock
    {
        public int ProductStockId { get; set; }
        public int Stock          { get; set; }
        public Product Product    { get; set; }
    }
}
