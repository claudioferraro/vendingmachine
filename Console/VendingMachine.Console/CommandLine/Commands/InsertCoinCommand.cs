
using Microsoft.Extensions.Localization;
using VendingMachine.Console.CommandLine.Commands.Interfaces;
using VendingMachine.Console.Model;
using VendingMachine.Console.Model.DTOs;
using VendingMachine.Console.Resources.ShowProduct;

namespace VendingMachine.Console.CommandLine.Commands
{
    public class InsertCoinCommand : CommandBase, IInsertCoinCommand
    {
        private readonly IStringLocalizer<InsertCoinResource> _localizer;

        private readonly IWallet _wallet;

        string[] allowedCommands = new string[] { "DECIMAL", "SHOW", "RETURN COINS" };

        int maxCoins = 20;

        public InsertCoinCommand(IStringLocalizer<InsertCoinResource> localizer, IWallet wallet)
        {
            _wallet = wallet;
            _localizer = localizer;
        }

        public override bool executeCommand()
        {
            IO.ClearScreen();
            IO.WriteLine(_localizer["Instruction"], 1);
            IO.WriteLine(_localizer["InsertCoin"], 1);
            Tuple<string, string> inputKeyValue;

            do
            {
                maxCoins--;
                inputKeyValue = IO.CatchMany(allowedCommands);
                switch (inputKeyValue.Item1)
                {
                    case "DECIMAL":
                        if (!addValueToWallet(inputKeyValue.Item2))
                            IO.Write(_localizer["CoinNotValid"]);
                        else displayWalletStatus();
                        break;

                    case "SHOW": return GoToNext();
                    case "RETURN COINS": inputKeyValue = null; return GoToBackup();
                    default: IO.Write(_localizer["CoinNotValid"]); break;
                }

                if (maxCoins == 0) IO.Write(_localizer["Rich"]);
            } while (inputKeyValue is not null);

            IO.CatchEnter();
            return GoToNext();
        }

        private bool addValueToWallet(string coin) => 
            _wallet.AddCoin(new CoinDTO
                (IOParser.ParseDecimal(coin)));

        private void displayWalletStatus() => 
            IO.WriteLine($"{String.Format(_localizer["AmountEntered"], _wallet.Credit)}", 1);
    }
}