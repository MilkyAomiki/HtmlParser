using HtmlAnalyzer.Parser.Extensions;
using HtmlAnalyzer.Parser.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HtmlAnalyzer.CLI.Interactors
{
	///<summary>
	/// Parses input and shows user an output.
	///</summary>
	internal class HtmlAnalyzerInteractor : Interactor
	{
		private readonly IHtmlParserService htmlParserService;
		private readonly ILogger logger;
		private readonly IFileFetchService fileFetchService;

		public HtmlAnalyzerInteractor(IHtmlParserService htmlParserService, ILogger<Interactor> logger, IFileFetchService fileFetchService) =>
			(this.htmlParserService, this.logger, this.fileFetchService) =
			(htmlParserService, logger, fileFetchService);

		///<summary>
		/// Start the program, parse the input and show an output.
		///</summary>
		public void Run(string filename)
		{
			logger.LogInformation($"File: {filename}");
			var stream = FetchFile(filename);

			string visibleText = ParseFile(stream);

			string[] words = SplitToWords(visibleText.ToLower());

			CountWords(words);
		}

		private void CountWords(string[] words)
		{
			logger.LogInformation("Counting words...");

			IDictionary<string, int> countedWords = words.CountUpWords();

			countedWords =
				countedWords
					.OrderByDescending(d => d.Value)
					.ToDictionary(k => k.Key, k => k.Value);

			logger.LogInformation("Done.");

			foreach (var map in countedWords)
				Print($"{map.Key}: {map.Value}");
		}

		private string[] SplitToWords(string text)
		{
			string[] words;

			logger.LogInformation("Splitting to words...");
			words = text.SplitToWords();

			logger.LogInformation($"Words count: {words.Length}\n");

			return words;
		}

		private string ParseFile(StreamReader stream)
		{
			logger.LogInformation("Extractig file content...");
			string content = stream.ReadToEnd();

			logger.LogInformation("Parsing...");
			string visibleText = htmlParserService.GetVisibleText(content);

			logger.LogInformation(visibleText);

			return visibleText;
		}

		private StreamReader FetchFile(string uri)
		{
			logger.LogInformation("Fetching html page...");
			var stream = fileFetchService.Fetch(uri);
			return stream;
		}
	}
}
