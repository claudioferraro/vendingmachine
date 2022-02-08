
using VendingMachine.Console.CommandLine.CommandComposite;
using VendingMachine.Console.CommandLine.Commands;

namespace VendingMachine.Console.VendingMachine
{
    public class VendingMachineInstance : VendingMachineBase, IVendingMachineInstance
    {
        private readonly CommandFactory _commandFactory;
        private ICommand rootCommand;

        public VendingMachineInstance(ICommandFactory commandFactory)
        {
            _commandFactory = (CommandFactory)commandFactory;
        }

        public void Init() 
        {
            rootCommand = _commandFactory.CreateSequence();
            rootCommand.Execute();
            return;
        }
    }
}
