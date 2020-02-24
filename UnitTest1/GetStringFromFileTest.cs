using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;

namespace UnitTest1
{
	[TestClass]
	public class GetStringFromFileTest
	{
		HtmlTools tool = new HtmlTools();

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
			Actual = tool.GetString(FilePath);

			//Assert
			Assert.AreEqual(Expected, Actual);

		}
	}
}
