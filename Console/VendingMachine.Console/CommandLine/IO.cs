using System.Text;
using SConsole = System.Console;

namespace VendingMachine.Console.CommandLine
{

    public static class IOParser
    {
        public static int ParseNumber(string input)
        {
            int outNumber = 0;
            Int32.TryParse(input, out outNumber);
            return outNumber;
        }

        public static decimal ParseDecimal(string input)
        {
            decimal num = decimal.Parse(input);
            string str  = num.ToString("N2");
            return decimal.Parse(str);
        }
    }

    public class IO : IIO
    {
        public int      CatchNumber()   => IOParser.ParseNumber(Catch());
        public decimal  CatchDecimal()  => IOParser.ParseDecimal(Catch());
        public void     ClearScreen()   => SConsole.Clear();
        public string   Catch()         => SConsole.ReadLine();

        public void CatchEnter()
        {
            ConsoleKeyInfo key;
            do
            {
                while (!SConsole.KeyAvailable)
                    Thread.Sleep(250); // Loop until input is entered.
                key = SConsole.ReadKey(true);
            } while (key.Key != ConsoleKey.Enter);
        }

        public Tuple<string, string> CatchMany(string[] commands)
        {
            var input = Catch();
            if (decimal.TryParse(input, out _))
                return Tuple.Create("DECIMAL", input);

            if (int.TryParse(input, out _))
                return Tuple.Create("INT", input);

            foreach (var command in commands)
                if (input == command)
                    return Tuple.Create(command, input);

            return Tuple.Create(String.Empty, String.Empty);
        }
        
        public void Write(string line) => SConsole.WriteLine(line);
        
        public void WriteLine(string line, int numOfnewLines = 1)
        {
            var str = new StringBuilder();
            str.Append(line);

            for (int i = 0; i < numOfnewLines; i++)
                str.Append(Environment.NewLine);
            
            SConsole.WriteLine(str.ToString());
        }
    }
}
