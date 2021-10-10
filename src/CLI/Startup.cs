using System;
using System.IO;
using HtmlAnalyzer.CLI.Config;
using HtmlAnalyzer.CLI.Interactors;
using HtmlAnalyzer.Parser.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace HtmlAnalyzer.CLI
{
	public class Startup
	{
		public void Start(string filename, bool verbose, bool logToFile)
		{
			using var services = GetServices(verbose, logToFile);

			try
			{
				services.GetService<HtmlAnalyzerInteractor>().Run(filename);
			}
			catch (ParserException e)
			{
				if (e.InnerException is not null)
					services.GetService<ILogger<Startup>>().LogCritical($"{e.Message}\n{e.InnerException.Message}");
				else
					services.GetService<ILogger<Startup>>().LogCritical(e.Message);

				PrintError($"An exception occured: {e.Message}");
			}
			catch (Exception e)
			{
				services.GetService<ILogger<Startup>>().LogCritical(e.Message);
				PrintError($"An exception occured");
			}
		}

		public ServiceProvider GetServices(bool logToConsole, bool logToFile)
		{
			var serviceCollection = new ServiceCollection();

			if (logToFile) serviceCollection.AddLogging(ConfigureFileLogging);
			if (logToConsole) serviceCollection.AddLogging(ConfigureConsoleLogging);

			if (!logToFile && !logToConsole) serviceCollection.AddLogging(SilentLogging);

			AddServices(serviceCollection);

			return serviceCollection.BuildServiceProvider();
		}

		public IServiceCollection AddServices(IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient<HtmlAnalyzerInteractor>();
			serviceCollection.AddParserServices();

			return serviceCollection;
		}

		public void ConfigureFileLogging(ILoggingBuilder builder)
		{
			var serilogLogger =
				new LoggerConfiguration()
					.MinimumLevel.Verbose()
					.Enrich.FromLogContext()
					.WriteTo.File(
						Path.Join("logs", $"html-parser-{DateTime.Now.ToString("yyyymmddhhmmss")}.log"),
						outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} {SourceContext} [{Level:u3}] {Message}{NewLine}{Exception}")
					.CreateLogger();

			builder
				.AddFilter("Microsoft", LogLevel.Warning)
				.AddFilter("System", LogLevel.Warning)
				.AddSerilog(serilogLogger, dispose: true); // Instead of .AddFile() - to flush and dispose the logger properly
		}

		public void ConfigureConsoleLogging(ILoggingBuilder builder)
		{
			builder
				.AddFilter("Microsoft", LogLevel.Warning)
				.AddFilter("System", LogLevel.Warning)
				.AddConsole();
		}

		public void SilentLogging(ILoggingBuilder builder)
		{
			builder
				.SetMinimumLevel(LogLevel.None);
		}

		public void PrintError(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Error.WriteLine(message);
			Console.ResetColor();
		}
	}
}
