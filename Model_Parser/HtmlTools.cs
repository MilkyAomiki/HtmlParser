using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Syroot.Windows.IO;
using System.Runtime;

namespace Parser
{
	/// <summary>
	/// Модель приложения
	/// </summary>

    public class HtmlTools: IHtmlTools
    {
		readonly KnownFolder _folder = new KnownFolder(KnownFolderType.Downloads);

		private string DefaultFolder => _folder.Path;
		private string _customFolder;


		public string DefaultDirectory => DefaultFolder + "\\HtmlDocs";
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

		//Путь к последнему добавленному файлу
		public string HtmlFilePath { get; private set; }


        public void DownloadHtml(string uri)
	    {
		    var builder = new UriBuilder(uri);
		    var localUri = builder.Uri;

			using (var client = new WebClient())
			{
				var directory = Directory.Exists(CustomDirectory) ? CustomDirectory : DefaultDirectory;
				if (directory == DefaultDirectory)
				{
					Directory.CreateDirectory(DefaultDirectory);
				}
				HtmlFilePath = directory + $"\\({localUri.Host}).html";
				    
				if (File.Exists(HtmlFilePath))
				{
					int cloneId = 0;
					while (File.Exists(HtmlFilePath))
					{
						cloneId++;
						HtmlFilePath = directory + $"\\({localUri.Host})({cloneId}).html";
					}
				}

				client.DownloadFile(localUri, HtmlFilePath);
			}
	    }
		public void DownloadHtml(string uri, string folderPath)
		{
			var builder = new UriBuilder(uri);
			var localUri = builder.Uri;
			using (var client = new WebClient())
			{
				if (Directory.Exists(folderPath))
				{
					_customFolder = folderPath;
					Directory.CreateDirectory(CustomDirectory);
                    HtmlFilePath = CustomDirectory + $"\\({localUri.Host}).html";

					client.DownloadFile(localUri, HtmlFilePath);
				}
				else
				{
					throw new DirectoryNotFoundException();
				}
			}
		}

		//Получает весь текст из файла
		public string GetString()
		{
			var content = File.Exists(HtmlFilePath) ? File.ReadAllText(HtmlFilePath) : "File doesn't exist";
			return content;
		}
		public string GetString(string filePath)
		{
			long b = new FileInfo(filePath).Length;
			long kb = b / 1024;
			long mb = kb / 1024;
			string content;
			mb = mb == 0 ? 1 : mb;

			if (!File.Exists(filePath)) throw  new FileNotFoundException();
			if (new FileInfo(filePath).Extension != ".html") throw new FileFormatException();
			try
			{
				MemoryFailPoint f = new MemoryFailPoint((int)mb);
			}
			catch (Exception e)
			{
				throw new InsufficientMemoryException();
			}
			
			content = File.ReadAllText(filePath);
			return content;
		}

		public string GetVisibleText(string html)
		{

			string visibleText = null;

			var withoutTitle   = Regex.Replace(html, @"<title[\W\w\S\s]*?>[\W\w\S\s]*?</title>", " ");
			var withoutScripts = Regex.Replace(withoutTitle, @"<script[\W\w\S\s]*?>[\W\w\S\s]*?</script>", " ");
            var withoutStyles  = Regex.Replace(withoutScripts, @"<style[\W\w\S\s]*?>[\W\w\S\s]*?</style>", " ");
            var withoutTags    = Regex.Replace(withoutStyles, @"<[\W\w\S\s]*?>", " ");
			var withoutSymbols = Regex.Replace(withoutTags, @"&[\W\w\S]*?;", " ");
			var withoutNewLine = Regex.Replace(withoutSymbols, @"\n", " ");

			for (int i = 0; i < withoutNewLine.Length; i++)
			{
				if (i+1 >= withoutNewLine.Length)
				{
					if (!Char.IsWhiteSpace(withoutNewLine[i]))
					{
						visibleText += withoutNewLine[i];
					}
					break;
				}
				if (Char.IsWhiteSpace(withoutNewLine[i + 1]) && Char.IsWhiteSpace(withoutNewLine[i]))
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

	    public IEnumerable<CountedWords> CountUpWord(string[] words)
	    {

		    var matches = words.Where(a => !String.IsNullOrEmpty(a)).GroupBy(x => x)
			    .Select(g => new CountedWords(g.Key, g.Count()));
			
		    return matches;
	    }
    }

	/// <summary>
	/// Объект:
	/// Слово - количество
	/// </summary>
	public class CountedWords
	{
		public string Word;
		public int Count;

		public CountedWords(string word, int count)
		{
			Word = word;
			Count = count;
		}
	}

}
