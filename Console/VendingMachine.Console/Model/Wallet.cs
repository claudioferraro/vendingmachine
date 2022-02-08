
using VendingMachine.Console.Model.DTOs;

namespace VendingMachine.Console.Model
{
    public class Wallet : IWallet
    {
        public decimal Credit { get; set; } = 0;

        private decimal[] allowedCoins = new decimal[] { 0.05M, 0.10M, 0.20M, 0.50M, 1.00M, 2.00M };

        public bool IsEnoughMoney(decimal price) => (price <= Credit);
        
        public bool AddCoin(CoinDTO coin)
        {
            if (ValidateCoin(coin))
                { Credit += coin.Value; return true; }
            return false;
        }

        public bool ValidateCoin(CoinDTO coin)
        {
            if (allowedCoins.Contains(coin.Value)) return true;
            return false;
        }

        public void ResetWallet() => Credit = 0;
    }
}
