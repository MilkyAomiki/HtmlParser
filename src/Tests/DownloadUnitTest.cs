using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using System.IO;

namespace Tests
{
	[TestClass]
	public class DownloadUnitTest
	{
		readonly HtmlTools tools = new HtmlTools();

		[TestMethod]
		public void DownloadToCustomFolder()
		{
			//Arrange
			var Uri = "simbirsoft.com";
			tools.CustomDirectory = "\\htmlDocs";

			//Act
			if (Directory.Exists(tools.CustomDirectory))
				Directory.Delete(tools.CustomDirectory, true);
			Directory.CreateDirectory(tools.CustomDirectory);
			tools.DownloadHtml(Uri);


			//Assert
			if (Directory.GetFiles(tools.CustomDirectory).Length == 0)
			{
				Assert.Fail("Custom directory is empty");
			}
		}

		[TestMethod]
		public void DownloadToDefaultFolder()
		{
			//Arrange
			var Uri = "simbirsoft.com";
			
			//Act
			tools.CustomDirectory = null;
			if (Directory.Exists(tools.DefaultDirectory))
				Directory.Delete(tools.DefaultDirectory, true);

			tools.DownloadHtml(Uri);


			//Assert

			if (Directory.GetFiles(tools.DefaultDirectory).Length == 0)
			{
				Assert.Fail("Default directory is empty");
			}

		}

	}
}
