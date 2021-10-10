namespace HtmlAnalyzer.Parser.Services
{
	/// <summary>
	/// Html analyzer
	/// </summary>
	public interface IHtmlParserService
	{
		/// <summary>
		/// Parses given HTML string and returns all visible text
		/// </summary>
		string GetVisibleText(string html);
	}
}
