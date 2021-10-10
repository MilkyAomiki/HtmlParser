using System.Text.RegularExpressions;

namespace HtmlAnalyzer.Parser.Tools
{
	internal static class HtmlFilteringTools
	{
		///<summary>
		/// Removes a full html tag along with everything in between its opening and closing tags
		///</summary>
		public static string RemoveTagWithContent(string html, string tag) =>
			Regex.Replace(html, @$"<{tag}[\W\w\S\s]*?>[\W\w\S\s]*?</{tag}>", " ");

		///<summary>
		/// Removes all html tags, leaving text untouched
		///</summary>
		public static string RemoveTagsSyntax(string html) =>
			Regex.Replace(html, @"<[\W\w\S\s]*?>", " ");
	}
}
