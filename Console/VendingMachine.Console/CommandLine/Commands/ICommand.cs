
namespace VendingMachine.Console.CommandLine.Commands
{
    public interface ICommand
    {
        public void     Execute();
        public ICommand SetNext(ICommand nextCommand);
        public ICommand SetBackup(ICommand nextBackup);
        public Boolean  HasNext();
        public Boolean  HasBackup();
    }
}
