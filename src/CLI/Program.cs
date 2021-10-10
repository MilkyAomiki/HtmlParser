using System.CommandLine;
using System.CommandLine.Invocation;

namespace HtmlAnalyzer.CLI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var cmd = new RootCommand
			{
				new Argument<string>("filename", "URI of the file to analyze.\nRelative paths are supported only for the file system."),
				new Option<bool>(new string[] { "--verbose", "-v" }, "Output logs"),
				new Option<bool>(new string[] { "--log-to-file", "-l"}, "Write logs to a file of the format 'logs/html-parser-yyyymmddhhmmss'")
			};

			cmd.Handler = CommandHandler.Create<string, bool, bool>(new Startup().Start);

			cmd.Invoke(args);
		}
	}
}
