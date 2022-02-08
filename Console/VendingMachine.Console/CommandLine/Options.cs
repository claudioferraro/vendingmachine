using CommandLine;

namespace VendingMachine.Console.CommandLine
{
	public class Options
	{
		[Option('r', "read", Required = false, HelpText = "Input files to be processed.")]
		public IEnumerable<string> InputFiles { get; set; }

		[Option('l', "lang", Required = false, Default = "EN", HelpText = "Change Language")]
		public string lang { get; set; }
	}
}
