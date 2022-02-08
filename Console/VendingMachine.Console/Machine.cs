using CommandLine;
using VendingMachine.Console.CommandLine;
using VendingMachine.Console.VendingMachine;

namespace VendingMachine.Console
{
    public class Machine : IMachine
    {
        private readonly VendingMachineFactory _vendingMachineFactory;

        public Machine(IVendingMachineFactory vendingMachineFactory)
        {
            _vendingMachineFactory = (VendingMachineFactory)vendingMachineFactory;
        }

        public void Run(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
              .WithParsed<Options>
                (o => {
                    OptionsSingleton.Instance = o;
                    _vendingMachineFactory.CreateMachine(); 
                });
        }
    }
}
