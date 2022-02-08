using VendingMachine.Console.CommandLine.Commands;
using VendingMachine.Console.CommandLine.Commands.Interfaces;

namespace VendingMachine.Console.CommandLine.CommandComposite
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IServiceProvider _serviceProvider;

        ICommand RootCommand;

        public CommandFactory(IServiceProvider serviceProvider) { _serviceProvider = serviceProvider; }

        private Tc newCommand<Ti, Tc>()
        {
            Type type = typeof(Ti);
            return (Tc)_serviceProvider.GetService(type);
        }

        public ICommand CreateSequence()
        {
            WelcomeCommand welcomeCommand = newCommand<IWelcomeCommand, WelcomeCommand>();
            InsertCoinCommand insertCoinCommand = newCommand<IInsertCoinCommand, InsertCoinCommand>();
            ShowProductCommand showProductCommand = newCommand<IShowProductCommand, ShowProductCommand>();
            MakeChangeCommand makeChangeCommand = newCommand<IMakeChangeCommand, MakeChangeCommand>();
            ReturnMoneyCommand returnMoneyCommand = newCommand<IReturnMoneyCommand, ReturnMoneyCommand>();
            FinalizeCommand finalizeCommand = newCommand<IFinalizeCommand, FinalizeCommand>();
            RootCommand = welcomeCommand;
            // welcome
            welcomeCommand.SetNext(insertCoinCommand);
            
            // insert coin
            insertCoinCommand.SetNext(showProductCommand);
            insertCoinCommand.SetBackup(returnMoneyCommand);

            // show product
            showProductCommand.SetNext(makeChangeCommand);
            showProductCommand.SetBackup(returnMoneyCommand);

            // make change
            makeChangeCommand.SetNext(returnMoneyCommand);
            makeChangeCommand.SetBackup(finalizeCommand);

            returnMoneyCommand.SetNext(finalizeCommand);
            returnMoneyCommand.SetBackup(finalizeCommand);

            return RootCommand;
        }
    }
}
