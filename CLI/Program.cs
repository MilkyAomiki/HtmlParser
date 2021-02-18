using Microsoft.Extensions.Logging;
using Parser;
using System;

namespace CLI
{
	class Program
	{
		static void Main(string[] args)
		{
			HtmlTools htmlTools = new HtmlTools();
			ILogger logger = GetLogger<Program>();

			HtmlAnalyzer htmlAnalyzer = new HtmlAnalyzer(htmlTools, logger, () => Exit(logger));
			htmlAnalyzer.Run();
		}

		private static void Exit(ILogger logger)
		{
			logger.LogInformation("Exiting...");
			Environment.Exit(Environment.ExitCode);
		}

		public static ILogger<T> GetLogger<T>()
		{
			using var loggerFactory = LoggerFactory.Create(builder =>
			{
				builder.AddFilter("Microsoft", LogLevel.Warning)
				.AddFilter("System", LogLevel.Warning)
				.AddConsole();
			});

			ILogger<T> logger = loggerFactory.CreateLogger<T>();
			return logger;
		}
	}
}
