
namespace VendingMachine.Console.CommandLine.Commands
{
    public abstract class CommandBase : ICommand
    {
        protected Options _options;
        protected ICommand NextCommand      { get; set; }
        protected ICommand BackupCommand    { get; set; }
        protected IO       IO               { get; set; }

        public    bool HasNext()    => NextCommand != null;
        public    bool HasBackup()  => BackupCommand != null;
        protected bool GoToBackup() => false;
        protected bool GoToNext()   => true;

        public CommandBase() { _options = OptionsSingleton.Instance; IO = new IO(); }

        public ICommand SetBackup(ICommand backupCommand)
        {
            BackupCommand = backupCommand;
            return backupCommand;
        }

        public ICommand SetNext(ICommand nextCommand)
        {
            NextCommand = nextCommand;
            return nextCommand;
        }

        public void Execute()
        {
            if (executeCommand()) 
                   NextCommand?.Execute();
            else 
                 BackupCommand?.Execute();//if (HasNext())
        }//if (HasBackup())

        public abstract bool executeCommand();
    }
}
