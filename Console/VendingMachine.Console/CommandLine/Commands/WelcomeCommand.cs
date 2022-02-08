using Microsoft.Extensions.Localization;
using VendingMachine.Console.CommandLine.Commands.Interfaces;
using VendingMachine.Console.Resources;

namespace VendingMachine.Console.CommandLine.Commands
{
    internal class WelcomeCommand : CommandBase, IWelcomeCommand
    {
        private readonly IStringLocalizer<WelcomeResource> _localizer = null!;

        public WelcomeCommand(IStringLocalizer<WelcomeResource> localizer) { _localizer = localizer; }

        public override bool executeCommand()
        {
            IO.Write(_localizer["Title"].Value);
            IO.Write(_localizer["Subtitle"].Value);
            IO.WriteLine(_localizer["Instruction"].Value, 2);
            IO.CatchEnter();
            return GoToNext();
        }
    }
}