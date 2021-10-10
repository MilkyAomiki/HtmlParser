using System.IO;

namespace HtmlAnalyzer.Parser.Services
{
	/// <summary>
	/// API for fetching a file from outside the app
	/// </summary>
	public interface IFileFetchService
	{
		/// <summary>
		/// Finds a file and opens a stream for reading its content
		/// </summary>
		StreamReader Fetch(string filename);
	}
}
