using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace UI
{
	/// <summary>
	/// Связывет HtmlTools и View, управляет окнами
	/// </summary>
	public partial class App : Application
	{
		private readonly ILogger logger;

		private readonly MainWindow w_main;
		private readonly ParserMain w_parser;
		private readonly IHtmlTools htmlTools = new HtmlTools();

		public App()
		{
			logger = GetLogger<App>();

			w_main = new MainWindow(logger);
			w_parser = new ParserMain(logger);
			
			DeserializeSettings();
			LinkViewEvents();
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

		private void LinkViewEvents()
		{
			w_main.SetCustomFolder += SetCustomFolder;
			w_main.GetCustomFolder += () => htmlTools.CustomDirectory;
			w_main.GetDefaultFolder += () => htmlTools.DefaultDirectory;
			w_main.BtnToParsePage_Click += OpenParseWindowAppear;
			w_main.Show();

			w_parser.DefaultDirectory = htmlTools.DefaultDirectory;
			w_parser.CustomDirectory = htmlTools.CustomDirectory;

			w_parser.LoadHtmlClick += LoadHtml;
			w_parser.OpenExtractPage += ShowLastFilePath;
			w_parser.ShowTextFromHtml_Click += ShowTextFromHtml;
			w_parser.SplitToWords += SplitToWords;
			w_parser.CountUpWord += CountUpWords;
		}

		private void DeserializeSettings()
		{
			if (File.Exists("settings.json"))
			{
				using StreamReader file = File.OpenText("settings.json");
				JsonSerializer serializer = new JsonSerializer();
				htmlTools.CustomDirectory = (string)serializer.Deserialize(file, typeof(string));
			}

		}

		private void SetCustomFolder(string path)
		{
			htmlTools.CustomDirectory = path;
			w_parser.CustomDirectory = htmlTools.CustomDirectory;
		}

		private void OpenParseWindowAppear()
		{
			w_main.Hide();
			w_parser.Closing += (sender, args) =>
			{
				args.Cancel = true;
				w_parser.Hide();
				w_main.Show();
			};
			w_parser.Owner = w_main;
			w_parser.Show();
		}

		private void LoadHtml(string html)
		{
			try
			{
				logger.LogInformation($"Downloading html {html}...");
				htmlTools.DownloadHtml(html);
				logger.LogInformation("Done.");
			}
			catch (Exception e)
			{
				logger.LogError($"Exception on downloading:\n {e.Message}\n");
			}

		}

		private string ShowLastFilePath()
		{
			return htmlTools.HtmlFilePath;
		}

		private string ShowTextFromHtml(string path)
		{
			string visibleText;
			try
			{
				logger.LogInformation($"Extracting content from {path}...");
				string html = htmlTools.GetText(path);
				logger.LogInformation("Done.");
				logger.LogInformation("Extracting visible text...");
				visibleText = htmlTools.GetVisibleText(html);
				logger.LogInformation("Done.");

			}
			catch (FileFormatException)
			{
				visibleText = "Wrong file extension";
				logger.LogError(visibleText);
			}
			catch (InsufficientMemoryException)
			{
				visibleText = "File is too large";
				logger.LogError(visibleText);
			}
			catch (FileNotFoundException)
			{
				visibleText = "File not found";
				logger.LogError(visibleText);
			}

			return visibleText;
		}

		private string[] SplitToWords(string text)
		{
			logger.LogInformation("Splitting to words...");
			string[] words = htmlTools.SplitToWords(text);
			logger.LogInformation($"Done. Words count:{words.Length}.");
			return words;
		}

		private IDictionary<string, int> CountUpWords(string[] words)
		{
			logger.LogInformation("Counting words...");
			IDictionary<string, int> wordsCount = htmlTools.CountUpWords(words);
			logger.LogInformation("Done.");
			return wordsCount;
		}
	}
}
