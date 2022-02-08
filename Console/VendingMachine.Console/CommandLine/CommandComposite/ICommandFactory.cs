
using VendingMachine.Console.CommandLine.Commands;

namespace VendingMachine.Console.CommandLine.CommandComposite
{
    public interface ICommandFactory
    {
        public ICommand CreateSequence();
    }
}
