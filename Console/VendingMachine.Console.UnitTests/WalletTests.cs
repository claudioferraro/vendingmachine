using NUnit.Framework;
using VendingMachine.Console.Model;
using VendingMachine.Console.Model.DTOs;

namespace VendingMachine.Console.UnitTests
{
    public class WalletTests
    {
        private Wallet _wallet;

        [SetUp]
        public void Setup()
        {
            _wallet = new Wallet();
        }

        [Test]
        public void AddCoinTest()
        {
            _wallet.AddCoin(new CoinDTO(25.22M));
            _wallet.AddCoin(new CoinDTO(25.226M));
            Assert.AreEqual(_wallet.Credit, 0M);
            _wallet.AddCoin(new CoinDTO(1.00M));
            Assert.AreEqual(_wallet.Credit, 1.00M);
        }

        [Test]
        public void AddCoinTest2()
        {
            _wallet.AddCoin(new CoinDTO(decimal.Parse("25.22")));
            _wallet.AddCoin(new CoinDTO(decimal.Parse("25.226")));
            Assert.AreNotEqual(_wallet.Credit, 50.446M);
            Assert.AreEqual(_wallet.Credit, 0.0M);
            decimal num = decimal.Parse("0.49999");
            string str = num.ToString("N2");

            _wallet.AddCoin(new CoinDTO(decimal.Parse(str)));
            Assert.AreEqual(_wallet.Credit, 0.50M);
        }

        [Test]
        public void ClearCoinTest()
        {
            _wallet.AddCoin(new CoinDTO(0.10M));
            _wallet.AddCoin(new CoinDTO(0.20M));
            _wallet.AddCoin(new CoinDTO(0.50M));
            _wallet.AddCoin(new CoinDTO(1.00M));
            _wallet.AddCoin(new CoinDTO(2.00M));
            Assert.AreNotEqual(_wallet.Credit, 25.22M);
            Assert.AreEqual(_wallet.Credit, 3.80M);
        }
    }
}
