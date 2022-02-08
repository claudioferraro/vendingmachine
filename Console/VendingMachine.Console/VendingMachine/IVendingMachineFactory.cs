using VendingMachine.Console.CommandLine;

namespace VendingMachine.Console.VendingMachine
{
    public interface IVendingMachineFactory 
    {
        public IVendingMachineInstance CreateMachine();
    }

}
