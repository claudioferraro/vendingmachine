using VendingMachine.Console.CommandLine;

namespace VendingMachine.Console.VendingMachine
{
    public class VendingMachineFactory : IVendingMachineFactory
    {
        public List<IVendingMachineInstance> VendingMachines { get; set; }

        private readonly IServiceProvider _serviceProvider;

        public VendingMachineFactory(IServiceProvider serviceProvider) 
        {
            
            _serviceProvider = serviceProvider;
            VendingMachines  = new List<IVendingMachineInstance>();
        }

        public IVendingMachineInstance CreateMachine()
        {
            VendingMachineInstance newInstance = 
                (VendingMachineInstance)_serviceProvider.GetService(typeof(IVendingMachineInstance));

            if (newInstance is not null)
                VendingMachines.Add(newInstance);

            newInstance.Init();
            return newInstance;
        }
    }
}
