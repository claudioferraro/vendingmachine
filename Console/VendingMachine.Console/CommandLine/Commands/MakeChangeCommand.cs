
using Microsoft.Extensions.Localization;
using VendingMachine.Console.CommandLine.Commands.Interfaces;
using VendingMachine.Console.Model;
using VendingMachine.Console.Resources.MakeChangeResource;

namespace VendingMachine.Console.CommandLine.Commands
{
    public class MakeChangeCommand : CommandBase, IMakeChangeCommand
    {
        private readonly IWallet _wallet;

        private readonly Change  _change;

        private readonly IStringLocalizer<MakeChangeResource> _localizer;

        public MakeChangeCommand(IStringLocalizer<MakeChangeResource> localizer, IChange change, IWallet wallet)
        {
            _change    = (Change)change;
            _wallet    = wallet;
            _localizer = localizer;
        }

        public override bool executeCommand()
        {
            var change = _change.MakeChange();
            if (change == -1) { IO.WriteLine(_localizer["Error"], 1); return GoToNext(); }

            _wallet.Credit -= change;
            // todo send the payment to server before reset
            _wallet.ResetWallet();

            IO.ClearScreen();
            IO.WriteLine(_localizer["Instruction"], 1);
            IO.WriteLine(String.Format(_localizer["Info"], change), 2);
            IO.Write(_localizer["PressEnter"]);
            IO.CatchEnter();
            return GoToNext();
        }
    }
}
