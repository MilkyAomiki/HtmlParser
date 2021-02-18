using System.Collections.Generic;

namespace Parser
{
	/// <summary>
	/// Html analyzator
	/// </summary>
	public interface IHtmlTools
	{
		string DefaultDirectory { get; }
		string CustomDirectory { get; set; }
		string HtmlFilePath { get; }

		/// <summary>
		/// Downloads html from the given <paramref name="uri"/>
		/// and saves to to <see cref="DefaultDirectory"/>
		/// </summary>
		/// <param name="uri"></param>
		void DownloadHtml(string uri);

		/// <summary>
		/// Downloads html from the given <paramref name="uri"/>
		/// and saves it to <paramref name="folderPath"/>
		/// </summary>
		void DownloadHtml(string uri, string folderPath);

		/// <summary>
		/// Gets all text from a file
		/// </summary>
		/// <returns><see cref="string"/> that contains all found text </returns>
		string GetText();

		/// <inheritdoc cref="GetText"/>
		string GetText(string filePath);

		/// <summary>
		/// Parses given html string 
		/// and returns all visible text from it
		/// </summary>
		string GetVisibleText(string html);

		/// <summary>
		/// Splits given text to words
		/// </summary>
		string[] SplitToWords(string text);

		/// <summary>
		/// Counts occurrence of every unique word in a given array of words
		/// </summary>
		/// <returns>
		/// <see cref="IDictionary{TKey, TValue}"/> where key contains word 
		/// and value contains its number of occurrence
		/// </returns>
		IDictionary<string, int> CountUpWords(string[] text);
	}
}
