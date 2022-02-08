using System.Text.Json.Serialization;
using VendingMachine.Domain.BoundedContext.Products.Model.Enums;

namespace VendingMachine.Domain.BoundedContext.Products.Model.ValueObjects
{
    public class Money : BaseValueObject
    {
        [JsonConstructor]
        public Money(Currency currency, decimal amount)
        {
            this.Currency = currency;
            this.Amount   = amount;
        }

        public Currency Currency { get; private set; }
        public decimal  Amount   { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Currency;
            yield return Amount;
        }
    }
}
