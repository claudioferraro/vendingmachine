using VendingMachine.Domain.BoundedContext.Products.Model.ValueObjects;

namespace VendingMachine.Domain.BoundedContext.Products.Model.Entities
{
    public class Product
    {
        public int Id             { get; set; }
        public string Name        { get; set; }
        public Money Price        { get; set; }
        public ProductStock Stock { get; set; }
    }
}
