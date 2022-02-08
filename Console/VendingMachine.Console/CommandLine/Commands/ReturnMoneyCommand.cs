using Microsoft.Extensions.Localization;
using VendingMachine.Console.CommandLine.Commands.Interfaces;
using VendingMachine.Console.Model;
using VendingMachine.Console.Resources.ReturnMoney;

namespace VendingMachine.Console.CommandLine.Commands
{
    public class ReturnMoneyCommand : CommandBase, IReturnMoneyCommand
    {
        private readonly Change  _change;

        private readonly IWallet _wallet;

        private readonly IStringLocalizer<ReturnMoneyResource> _localizer;

        public ReturnMoneyCommand(IChange change, IWallet wallet, IStringLocalizer<ReturnMoneyResource> localizer)
        {
            _change    = (Change)change;
            _localizer = localizer;
            _wallet    = wallet;
        }

        public override bool executeCommand()
        {
            IO.ClearScreen();
            decimal returnMoney = 0;
            if (_change.Value != 0)
                { returnMoney += _change.Value; IO.Write(String.Format(_localizer["Instruction"], returnMoney)); }
            if (_wallet.Credit != 0)
                { returnMoney += _wallet.Credit; IO.Write(String.Format(_localizer["Instruction2"], returnMoney)); }

            IO.CatchEnter();            
            _change.ResetChange();
            return GoToNext();
        }
    }
}
