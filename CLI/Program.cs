using Parser;
using System;

namespace CLI
{
	class Program
	{
		static void Main(string[] args)
		{

			IHtmlTools tools = new HtmlTools();
			Console.Write("Input Uri: ");
			string uri = Console.ReadLine();

			tools.DownloadHtml(uri);
			var str = tools.GetText();
			bool IsRead;
			Console.WriteLine("Parse File?");
			var input = Console.ReadLine();
			string visibleText = null;
			IsRead = input == "yes"||input == "ok" ;
			if (IsRead)
			{
				visibleText = tools.GetVisibleText(str);
				Console.WriteLine(visibleText);
			}
			Console.WriteLine("Split To Words?");
			var input2 = Console.ReadLine();

			IsRead = input2 == "yes" || input2 == "ok";
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
