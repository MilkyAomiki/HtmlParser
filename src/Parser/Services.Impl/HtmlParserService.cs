using System.Net;
using HtmlAnalyzer.Parser.Extensions;
using HtmlAnalyzer.Parser.Tools;

namespace HtmlAnalyzer.Parser.Services.Impl
{
	public class HtmlParserService : IHtmlParserService
	{
		public string GetVisibleText(string html)
		{
			var invisibleTags = new[] { "script", "style", "title" };

			foreach (var tag in invisibleTags)
				html = HtmlFilteringTools.RemoveTagWithContent(html, tag);

			html = HtmlFilteringTools.RemoveTagsSyntax(html);
			html = WebUtility.HtmlDecode(html);

			html = html.RemoveNewLines();
			html = html.RemoveRepeatingWhitespaces();
			html = html.Trim();

			return html;
		}
	}
}
