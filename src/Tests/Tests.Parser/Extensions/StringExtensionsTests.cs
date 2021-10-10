using HtmlAnalyzer.Parser.Extensions;
using Xunit;

namespace Tests.Parser.Services.Impl
{
	public class StringExtensionsTests
	{
		public class TheSplitToWordsMethod
		{
			[Theory]
			[InlineData(",")]
			[InlineData(" ")]
			[InlineData(".")]
			[InlineData(">")]
			[InlineData("+")]
			[InlineData("~")]
			public void Given_WordsSeparatedByCharacter_And_Character_Should_Return_Words(string delimiter)
			{
				//Arrange
				string[] words = { "The", "Story", "Begins", "In", "A", "Faraway", "Kingdom", "Where", "A", "Boy", "Lived" };
				string text = string.Join(delimiter, words);

				//Act
				var result = text.SplitToWords(delimiter);

				//Assert
				Assert.Equal(words, result);
			}

			[Fact]
			public void Given_WordsSeparatedByCharacters_And_Characters_Should_Return_Words()
			{
				//Arrange
				string[] words = { "The", "Story", "Begins", "In", "A", "Faraway", "Kingdom", "Where", "A", "Boy", "Lived" };
				string text = "The.Story+Begins>In>A>Faraway>Kingdom>Where~A Boy,Lived";

				//Act
				var result = text.SplitToWords(".+>~ ,");

				//Assert
				Assert.Equal(words, result);
			}
		}
	}
}
