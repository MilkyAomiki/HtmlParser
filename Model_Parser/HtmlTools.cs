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
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Common;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
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
		int CountUpWord(string text, string word);

	}

    public class HtmlTools: IHtmlTools
    {
		readonly KnownFolder _folder = new KnownFolder(KnownFolderType.Downloads);

		private string DefaultFolder => _folder.Path;
		public string DefaultDirectory => _folder.Path + "\\HtmlDocs";

		private string CustomFolder;
		public string CustomDirectory => CustomFolder + "\\HtmlDocs";

		private string _htmlFilePath;

		public void DownloadHtml(string uri)
	    {
		    var builder = new UriBuilder(uri);
			var localUri = builder.Uri;
			using (var client = new WebClient())
		    {
			    if (Directory.Exists(DefaultFolder))
			    {
				    Directory.CreateDirectory(DefaultDirectory);
				    _htmlFilePath = DefaultDirectory + $"\\({localUri.Host}).html";
				    
					if (File.Exists(_htmlFilePath))
				    {
						int cloneId = 0;
						while (File.Exists(_htmlFilePath))
						{
							cloneId++;
							_htmlFilePath = DefaultDirectory + $"\\({localUri.Host})({cloneId}).html";
						}
					}
					

					client.DownloadFile(localUri, _htmlFilePath);
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
					_htmlFilePath = CustomDirectory + $"\\({localUri.Host}).html";

					client.DownloadFile(localUri, _htmlFilePath);
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
			if (File.Exists(_htmlFilePath))
			{
				content = File.ReadAllText(_htmlFilePath);
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

			for (int i = 0; i < withoutTags.Length; i++)
			{
				if (i+1 >= withoutTags.Length)
				{
					break;
				}
				if (Char.IsWhiteSpace(withoutTags[i + 1]) && Char.IsWhiteSpace(withoutTags[i]))
				{
					continue;
				}

				visibleText += withoutTags[i];
			}

			return visibleText;
		}

	    public string[] SplitToWords(string text)
	    {
		    throw new NotImplementedException();
	    }

	    public int CountUpWord(string text, string word)
	    {
		    throw new NotImplementedException();
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
