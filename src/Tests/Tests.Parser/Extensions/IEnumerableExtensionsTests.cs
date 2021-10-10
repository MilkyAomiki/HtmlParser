using System.Collections.Generic;
using HtmlAnalyzer.Parser.Extensions;
using Xunit;

namespace Tests.Parser.Services.Impl
{
	public class IEnumerableExtensionsTests
	{
		public class TheCountUpWordsMethod
		{
			[Fact]
			public void Given_Words_Should_Return_WordsCount()
			{
				//Arrange
				IDictionary<string, int> exptected = new Dictionary<string, int>()
				{
					{ "The", 2 },
					{ "Story", 1 },
					{ "Begins", 1 },
					{ "In", 1 },
					{ "A", 2 },
					{ "Faraway", 1 },
					{ "Kingdom", 1},
					{ "Where", 1 },
					{ "Boy", 1 },
					{ "Lived", 1}
				};

				IEnumerable<string> words = new[] { "The", "The", "Story", "Begins", "In", "A", "Faraway", "Kingdom", "Where", "A", "Boy", "Lived", "", null };

				//Act
				var result = words.CountUpWords();

				//Assert
				Assert.Equal(exptected, result);
			}
		}
	}
}
