
using VendingMachine.Console.Model.DTOs;

namespace VendingMachine.Console.Model
{
    public class Change : IChange
    {
        public ProductDTO SelectedProduct { get; set; }
        public decimal Credit             { get; set; }
        public decimal TotalAmount        { get; set; }
        public decimal Value              { get; set; }

        public decimal MakeChange()
        {
            if (TotalAmount != SelectedProduct.Price)
                return -1;

            if (TotalAmount <= Credit)
            {
                Value = Credit - TotalAmount;
                return Value;
            }
            return -1;
        }

        public void ResetChange() { Value = 0; }
    }
}
