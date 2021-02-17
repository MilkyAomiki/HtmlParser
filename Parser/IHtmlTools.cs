using System.Collections.Generic;

namespace Parser
{
	/// <summary>
	/// Parser / analyzator
	/// </summary>
	public interface IHtmlTools
	{
		string DefaultDirectory { get; }
		string CustomDirectory { get; set; }
		string HtmlFilePath { get; }

		void DownloadHtml(string uri);
		void DownloadHtml(string uri, string folderPath);

		/// <summary>
		/// Gets all text from a file
		/// </summary>
		/// <returns><see cref="string"/> that contains all found text </returns>
		string GetText();

		/// <inheritdoc cref="GetText"/>
		string GetText(string filePath);

		string GetVisibleText(string html);
		string[] SplitToWords(string text);
		IEnumerable<CountedWords> CountUpWords(string[] text);
	}
}
