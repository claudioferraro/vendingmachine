
using VendingMachine.Console.Model.DTOs;

namespace VendingMachine.Console.Model
{
    public interface IWallet
    {
        public decimal Credit { get; set; }
        public bool IsEnoughMoney(decimal price);
        public bool AddCoin(CoinDTO coin);
        public bool ValidateCoin(CoinDTO coin);
        public void ResetWallet();
    }
}
