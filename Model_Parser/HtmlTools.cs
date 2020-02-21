using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using Syroot.Windows.IO;

namespace Model_Parser
{
	public interface IHtmlTools
	{
		void DownloadHtml(string uri);
		void DownloadHtml(string uri, string folderPath);
		string GetString();
		string GetString (string filePath);
		string GetVisibleText(string html);
		string[] SplitToWords(string text);
		int CountUpWord(string[] text, string word);

	}

    public class HtmlTools: IHtmlTools
    {
		readonly KnownFolder _folder = new KnownFolder(KnownFolderType.Downloads);

		private string DefaultFolder => _folder.Path;
		public string DefaultDirectory => _folder.Path + "\\HtmlDocs";

		private string CustomFolder;
		public string CustomDirectory => CustomFolder + "\\HtmlDocs";

        private string _htmlFilePath;
        public string HtmlFilePath { get => _htmlFilePath; private set => _htmlFilePath = value; }

       


		public void DownloadHtml(string uri)
	    {
		    var builder = new UriBuilder(uri);
			var localUri = builder.Uri;

			using (var client = new WebClient())
		    {
			    if (Directory.Exists(DefaultFolder))
			    {
				    Directory.CreateDirectory(DefaultDirectory);
                    HtmlFilePath = DefaultDirectory + $"\\({localUri.Host}).html";
				    
					if (File.Exists(HtmlFilePath))
				    {
						int cloneId = 0;
						while (File.Exists(HtmlFilePath))
						{
							cloneId++;
                            HtmlFilePath = DefaultDirectory + $"\\({localUri.Host})({cloneId}).html";
						}
					}
					
					client.DownloadFile(localUri, HtmlFilePath);
				}
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
					
					CustomFolder = folderPath;
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

		public string GetString()
		{
			string content;
			if (File.Exists(HtmlFilePath))
			{
				content = File.ReadAllText(HtmlFilePath);
			}
			else
			{
				content = "File doesn't exist";
			}

			return content;
		}

		public string GetString(string filePath)
		{
			string content;

			if (File.Exists(filePath))
			{
				content = File.ReadAllText(filePath);
			}
			else
			{
				content = "File doesn't exist";
			}
			return content;
		}

		public string GetVisibleText(string html)
		{

			string visibleText = null;

			var withoutTitle =   Regex.Replace(html, @"<title[\W\w\S\s]*?>[\W\w\S\s]*?</title>", " ");
			var withoutScripts = Regex.Replace(withoutTitle, @"<script[\W\w\S\s]*?>[\W\w\S\s]*?</script>", " ");
			var withoutTags =    Regex.Replace(withoutScripts, @"<[\W\w\S\s]*?>", " ");
			var withoutSymbols = Regex.Replace(withoutTags, @"&[\W\w\S]*?;", " ");
			var withoutNewLine = Regex.Replace(withoutSymbols, @"\n", " ");

			for (int i = 0; i < withoutNewLine.Length; i++)
			{
				if (i+1 >= withoutNewLine.Length)
				{
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
			string[] patterns = { " ", ",", ".", "", ")", "(", "\"", ":", ";", "?", "]", "["};
			var words = Regex.Split(text, "[ ,\\.!?:;\\]\\[\\)\\(\\n\\r\\t\"«»]+");

			//var withoutCharacters = words.Except(patterns).ToArray();

		    return words;
	    }

	    public int CountUpWord(string[] text, string word)
	    {
		    var matches = text.Where(x => x == word);
		    return matches.Count();
	    }

	    private void DeleteDefaultDirectory()
	    {
			Directory.Delete(DefaultDirectory, true);
	    }
	    private void DeleteCustomDirectory()
	    {
		    Directory.Delete(CustomDirectory, true);
	    }


    }
}
