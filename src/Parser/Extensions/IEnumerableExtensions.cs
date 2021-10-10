using System.Collections.Generic;
using System.Linq;

namespace HtmlAnalyzer.Parser.Extensions
{
	public static class IEnumerableExtensions
	{
		///<summary>
		/// Counts occurrence of every unique word in a given array of words
		///</summary>
		///<returns>
		/// Mappings word : count
		///</returns>
		public static IDictionary<string, int> CountUpWords(this IEnumerable<string> words) =>
			words
				.Where(a => !string.IsNullOrEmpty(a))
				.GroupBy(x => x)
				.ToDictionary(g => g.Key, g => g.Count());
	}
}
