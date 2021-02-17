using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using System.IO;

namespace Tests
{
	[TestClass]
	public class GetStringFromFileTest
	{
		readonly HtmlTools tool = new HtmlTools();

		[TestMethod]
		public void GetStringTest()
		{
			//Arrange
			string FilePath = "testFile.html";
			string Expected = "Hello World!";
			string Actual;

			//Act
			using (StreamWriter writer = File.CreateText(FilePath))
			{
				writer.Write(Expected);
			}
			Actual = tool.GetText(FilePath);

			//Assert
			Assert.AreEqual(Expected, Actual);

		}
	}
}
