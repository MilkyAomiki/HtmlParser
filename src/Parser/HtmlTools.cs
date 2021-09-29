using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Runtime;

namespace Parser
{
	public class HtmlTools : IHtmlTools
	{
		private string _customFolder;
		private readonly string _defaultDirectory = "HtmlDocs";

		public string DefaultDirectory => Path.GetFullPath(_defaultDirectory);

		/// <summary>
		/// Путь к последнему добавленному файлу
		/// </summary>
		public string HtmlFilePath { get; private set; }

		public string CustomDirectory
		{
			get => _customFolder;

			set
			{
				_customFolder = value;
				if (File.Exists(value))
				{
					Directory.CreateDirectory(CustomDirectory);
				}
			}
		}

		public void DownloadHtml(string uri)
		{
			var builder = new UriBuilder(uri);
			var localUri = builder.Uri;

			PrepareDirectory(localUri.Host);
			using (var client = new WebClient())
			{
				client.DownloadFile(localUri, HtmlFilePath);
			}
		}

		public void DownloadHtml(string uri, string folderPath)
		{
			var builder = new UriBuilder(uri);
			var localUri = builder.Uri;

			PrepareDirectory(localUri.Host, folderPath);
			using (var client = new WebClient())
			{
				client.DownloadFile(localUri, HtmlFilePath);
			}
		}

		/// <summary>
		/// Получает весь текст из файла
		/// </summary>
		public string GetText()
		{
			var content = File.Exists(HtmlFilePath) ? File.ReadAllText(HtmlFilePath) : "File doesn't exist";
			return content;
		}

		public string GetText(string filePath)
		{
			long b = new FileInfo(filePath).Length;
			long kb = b / 1024;
			long mb = kb / 1024;
			string content;
			mb = mb == 0 ? 1 : mb;

			if (!File.Exists(filePath)) throw new FileNotFoundException();
			if (new FileInfo(filePath).Extension != ".html") throw new FileLoadException();
			try
			{
				MemoryFailPoint f = new MemoryFailPoint((int)mb);
			}
			catch (Exception)
			{
				throw new InsufficientMemoryException();
			}

			content = File.ReadAllText(filePath);
			return content;
		}

		public string GetVisibleText(string html)
		{
			string visibleText = null;

			var withoutTitle = Regex.Replace(html, @"<title[\W\w\S\s]*?>[\W\w\S\s]*?</title>", " ");
			var withoutScripts = Regex.Replace(withoutTitle, @"<script[\W\w\S\s]*?>[\W\w\S\s]*?</script>", " ");
			var withoutStyles = Regex.Replace(withoutScripts, @"<style[\W\w\S\s]*?>[\W\w\S\s]*?</style>", " ");
			var withoutTags = Regex.Replace(withoutStyles, @"<[\W\w\S\s]*?>", " ");
			var withoutSymbols = Regex.Replace(withoutTags, @"&[\W\w\S]*?;", " ");
			var withoutNewLine = Regex.Replace(withoutSymbols, @"\n", " ");

			for (int i = 0; i < withoutNewLine.Length; i++)
			{
				if (i + 1 >= withoutNewLine.Length)
				{
					if (!char.IsWhiteSpace(withoutNewLine[i]))
					{
						visibleText += withoutNewLine[i];
					}
					break;
				}
				if (char.IsWhiteSpace(withoutNewLine[i + 1]) && char.IsWhiteSpace(withoutNewLine[i]))
				{
					continue;
				}

				visibleText += withoutNewLine[i];
			}

			return visibleText;
		}

		public string[] SplitToWords(string text)
		{
			text = text.ToLower();
			var words = Regex.Split(text, "[ ,\\.!?:;\\]\\[\\)\\(\\n\\r\\t\"«»]+");
			return words;
		}

		public IDictionary<string, int> CountUpWords(string[] words)
		{
			var matches = words.Where(a => !string.IsNullOrEmpty(a))
				.GroupBy(x => x)
				.ToDictionary(g => g.Key, g => g.Count());

			return matches;
		}

		private void PrepareDirectory(string fileName, string directoryPath)
		{
			if (Directory.Exists(directoryPath))
			{
				_customFolder = directoryPath;
				Directory.CreateDirectory(CustomDirectory);
				HtmlFilePath = CustomDirectory + $"\\({directoryPath}).html";
			}
			else
			{
				throw new DirectoryNotFoundException();
			}
		}

		private void PrepareDirectory(string fileName)
		{
			var directory = Directory.Exists(CustomDirectory) ? CustomDirectory : DefaultDirectory;
			if (directory == DefaultDirectory)
			{
				Directory.CreateDirectory(DefaultDirectory);
			}

			HtmlFilePath = directory + $"\\({fileName}).html";

			if (File.Exists(HtmlFilePath))
			{
				int cloneId = 0;
				while (File.Exists(HtmlFilePath))
				{
					cloneId++;
					HtmlFilePath = directory + $"\\({fileName})({cloneId}).html";
				}
			}
		}
	}
}
