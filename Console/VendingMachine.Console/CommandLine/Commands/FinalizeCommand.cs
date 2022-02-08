using Microsoft.Extensions.Localization;
using VendingMachine.Console.CommandLine.CommandComposite;
using VendingMachine.Console.CommandLine.Commands.Interfaces;
using VendingMachine.Console.Model;
using VendingMachine.Console.Resources.FinalizeResource;

namespace VendingMachine.Console.CommandLine.Commands
{
    public class FinalizeCommand : CommandBase, IFinalizeCommand
    {
        private readonly Change _change;

        private readonly IStringLocalizer<FinalizeResource> _localizer;

        private readonly ICommandFactory _commandFactory;
        public FinalizeCommand(IChange change, IStringLocalizer<FinalizeResource> localizer, ICommandFactory commandFactory)
        {
            _change         = (Change)change;
            _localizer      = localizer;
            _commandFactory = commandFactory;
        }

        public override bool executeCommand()
        {
            IO.Write(String.Format(_localizer["Information"], _change.SelectedProduct.Name));
            IO.Write(_localizer["GoodByeMessage"]);
            _change.SelectedProduct = null;
            IO.Write(_localizer["Information2"]);

            IO.CatchEnter();
            IO.ClearScreen();

            ICommand rootCommand = _commandFactory.CreateSequence();
            rootCommand.Execute();
            return GoToNext();
        }
    }
}
