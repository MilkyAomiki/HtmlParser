using Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CLI
{
	class Program
	{
		static void Main(string[] args)
		{
			IHtmlTools tools = new HtmlTools();
			Console.Write("Enter Uri: ");
			string uri = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(uri))
			{
				Console.Error.WriteLine("Input is empty");
				return;
			}

			tools.DownloadHtml(uri);
			var str = tools.GetText();

			Console.WriteLine("Parse File? (Y/n)");
			var answerInp = Console.ReadLine();

			string visibleText;

			if (IsTrueInput(answerInp, true))
			{
				visibleText = tools.GetVisibleText(str);
				Console.WriteLine(visibleText + "\n");
			}
			else
			{
				return;
			}

			Console.WriteLine("Split To Words? (Y/n)");
			answerInp = Console.ReadLine();

			string[] words;

			if (IsTrueInput(answerInp, true))
			{
				words = tools.SplitToWords(visibleText);
				foreach (var i in words)
				{
					Console.WriteLine(i);
				}

				Console.WriteLine();

			}
			else
			{
				return;
			}

			Console.WriteLine("Count words? (Y/n)");
			answerInp = Console.ReadLine();

			if (IsTrueInput(answerInp, true))
			{
				IDictionary<string, int> countedWords = tools.CountUpWords(words);
				countedWords = countedWords.OrderByDescending(d => d.Value)
					.ToDictionary(k => k.Key, k => k.Value);

				foreach (var map in countedWords)
				{
					Console.WriteLine($"{map.Key}: {map.Value}");
				}

				Console.WriteLine();
			}


			Console.Write("Press any key to continue...");
			Console.Read();
		}

		private static bool IsTrueInput(string input, bool emptyAllowed = false)
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
