using System.Text;
using System.Text.RegularExpressions;

namespace HtmlAnalyzer.Parser.Extensions
{
	public static class StringExtensions
	{
		///<summary>
		/// Splits a given string into words
		///</summary>
		///<param name="delimeters">The list of characters to use as separators for the string </param>
		///<returns>
		/// Mappings word : count
		///</returns>
		public static string[] SplitToWords(this string text, string delimeters = " ,\\.!?:;\\]\\[\\)\\(\\n\\r\\t\"«»") =>
			Regex.Split(text, $"[{delimeters}]+");

		/// <summary>
		/// Removes whitespaces repeating in a row, for example:
		/// It turns 'French     croissant' into 'French croissant'
		/// </summary>
		public static string RemoveRepeatingWhitespaces(this string text)
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < text.Length; i++)
			{
				if (i == text.Length - 1)
				{
					if (!char.IsWhiteSpace(text[i]))
						sb.Append(text[i]);

					break;
				}

				if (char.IsWhiteSpace(text[i + 1]) && char.IsWhiteSpace(text[i]))
					continue;

				sb.Append(text[i]);
			}

			return sb.ToString();
		}

		public static string RemoveNewLines(this string html) =>
			Regex.Replace(html, @"\n", " ");
	}
}

