using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Model_Parser;

namespace TestUi
{
	class Program
	{
		static void Main(string[] args)
		{

			IHtmlTools tools = new HtmlTools();
			Console.Write("Input Uri: ");
			string uri = Console.ReadLine();

			tools.DownloadHtml(uri);
			var str = tools.GetString();
			bool IsRead;
			Console.WriteLine("Parse File?");
			var input = Console.ReadLine();
			string visibleText = null;
			IsRead = (input == "yes"||input == "ok" )? true : false;
			if (IsRead)
			{
				visibleText = tools.GetVisibleText(str);
				Console.WriteLine(visibleText);
			}
			Console.WriteLine("Split To Words?");
			var input2 = Console.ReadLine();

			IsRead = (input2 == "yes" || input2 == "ok") ? true : false;
			string[] words;
			if (IsRead)
			{
				
				words = tools.SplitToWords(visibleText);
				foreach (var i in words)
				{
					Console.WriteLine(i);
				}

			}
			Console.Read();
		}
	}
}
