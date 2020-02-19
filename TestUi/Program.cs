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

			bool IsRead;
			Console.WriteLine("ReadFile?");
			var input = Console.ReadLine();

			IsRead = (input == "yes"||input == "ok" )? true : false;
			if (IsRead)
			{
				Console.WriteLine(tools.GetString());
			}
			Console.Read();
		}
	}
}
