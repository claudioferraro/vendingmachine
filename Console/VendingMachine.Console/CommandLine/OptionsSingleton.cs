
namespace VendingMachine.Console.CommandLine
{
    public class OptionsSingleton : IOptionsSingleton
    {
        private static Options instance = null;
        private static readonly object padlock = new object();

        public static Options Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Options();
                    }
                    return instance;
                }
            }
            set 
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = value;
                    }
                }
            }
            
        }
    }
}
