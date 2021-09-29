using Microsoft.Extensions.Logging;
using Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CLI
{
	internal class HtmlAnalyzer
	{
		private readonly IHtmlTools htmlTools;
		private readonly ILogger logger;
		private readonly Action stopCallback;

		public HtmlAnalyzer(IHtmlTools htmlTools, ILogger logger, Action stopCallback)
		{
			this.htmlTools = htmlTools;
			this.logger = logger;
			this.stopCallback = stopCallback;
		}

		public void Run()
		{
			string url = GetUrlInput();
			DownloadPage(url);

			string visibleText = ParseFile();
			string[] words = SplitToWords(visibleText);
			CountWords(words);

			Exit();
		}

		private void Exit()
		{
			Console.Write("Press any key to continue...");
			Console.Read();
			stopCallback();
		}

		private void CountWords(string[] words)
		{
			Console.WriteLine("Count words? (Y/n)");
			string answerInp = Console.ReadLine();

			if (IsTrueInput(answerInp, true))
			{
				logger.LogInformation("Counting words...");

				IDictionary<string, int> countedWords = htmlTools.CountUpWords(words);
				countedWords = countedWords.OrderByDescending(d => d.Value)
					.ToDictionary(k => k.Key, k => k.Value);

				logger.LogInformation("Done.");

				foreach (var map in countedWords)
				{
					Console.WriteLine($"{map.Key}: {map.Value}");
				}

				Console.WriteLine();
			}
		}

		private string[] SplitToWords(string text)
		{
			Console.WriteLine("Split To Words? (Y/n)");
			string answerInp = Console.ReadLine();

			string[] words;

			if (IsTrueInput(answerInp, true))
			{
				logger.LogInformation("Splitting to words...");
				words = htmlTools.SplitToWords(text);
				logger.LogInformation("Done.");

				Console.WriteLine($"Words count: {words.Length}\n");
				foreach (var i in words)
				{
					Console.WriteLine(i);
				}

				Console.WriteLine();
				return words;
			}
			else
			{
				stopCallback();
				return null;
			}
		}

		private string ParseFile()
		{
			logger.LogInformation("Extractig file content...");
			string content = htmlTools.GetText();
			logger.LogInformation("Done.");


			Console.WriteLine("Parse File? (Y/n)");
			var answerInp = Console.ReadLine();

			if (IsTrueInput(answerInp, true))
			{
				logger.LogInformation("Parsing...");
				string visibleText = htmlTools.GetVisibleText(content);
				logger.LogInformation("Done.");

				Console.WriteLine(visibleText + "\n");

				return visibleText;
			}
			else
			{
				stopCallback();
				return null;
			}

		}

		private string GetUrlInput()
		{
			Console.Write("Enter Uri: ");
			string url = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(url))
			{
				logger.LogError("Given url is empty");
				throw new ArgumentException("Empty input", nameof(url));
			}

			return url;
		}

		private void DownloadPage(string uri)
		{
			logger.LogInformation("Downloading html page...");
			htmlTools.DownloadHtml(uri);
			logger.LogInformation("Done.");
		}

		private bool IsTrueInput(string input, bool emptyAllowed = false)
		{
			input = input.ToLower();
			if (input == "y" || input == "yes")
				return true;


			if (emptyAllowed && string.IsNullOrWhiteSpace(input))
				return true;

			return false;
		}
	}
}
