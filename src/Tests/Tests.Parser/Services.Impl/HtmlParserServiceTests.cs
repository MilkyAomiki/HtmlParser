using HtmlAnalyzer.Parser.Services.Impl;
using Xunit;

namespace Tests.Parser.Services.Impl
{
	public class HtmlParserServiceTests
	{
		public class TheGetVisibleTextMethod
		{
			[Fact]
			public void Given_SimpleTag_Should_Return_VisibleText()
			{
				//Arrange
				string visibleText = "The Story";
				string html = $"<h1>{visibleText}<h1>";
				var service = new HtmlParserService();

				//Act
				var result = service.GetVisibleText(html);

				//Assert
				Assert.Equal(visibleText, result);
			}

			[Fact]
			public void Given_SimpleTag_With_HtmlEntities_Should_Return_EncodedVisibleText()
			{
				//Arrange
				string encodedVisibleText = "&#165;1234";
				string decodedVisibleText = "¥1234";
				string html = $"<h1>{encodedVisibleText}<h1>";
				var service = new HtmlParserService();

				//Act
				var result = service.GetVisibleText(html);

				//Assert
				Assert.Equal(decodedVisibleText, result);
			}

			[Theory]
			[InlineData("script")]
			[InlineData("style")]
			[InlineData("title")]
			public void Given_FunctionalTag_Should_Return_EmptyString(string tagName)
			{
				//Arrange
				string html = $"<{tagName}>it doesn't matter what you put here</{tagName}>";
				var service = new HtmlParserService();

				//Act
				var result = service.GetVisibleText(html);

				//Assert
				Assert.Equal(string.Empty, result);
			}

			[Fact]
			public void Given_MultilineHtml_Should_Return_OnelineVisibleText()
			{
				//Arrange
				string visibleText = "The Story\nbegins long ago\nin a faraway kingdom";
				string onelineVisibleText = "The Story begins long ago in a faraway kingdom";
				string html = $"<h1>{visibleText}<h1>";
				var service = new HtmlParserService();

				//Act
				var result = service.GetVisibleText(html);

				//Assert
				Assert.Equal(onelineVisibleText, result);
			}

			[Fact]
			public void Given_MultipleWhitespacesBetweenWords_Should_Return_SingleWhitespaceBetweenWords()
			{
				//Arrange
				string multipleWhitepaces = "french   croissant";
				string singleWhitespace = "french croissant";
				var service = new HtmlParserService();

				//Act
				var result = service.GetVisibleText(multipleWhitepaces);

				//Assert
				Assert.Equal(singleWhitespace, result);
			}
		}
	}
}
