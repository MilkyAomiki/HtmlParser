using System.Collections.Generic;

namespace Parser
{
    public interface IHtmlTools
	{
        string DefaultDirectory { get; }
        string CustomDirectory { get; set; }
        string HtmlFilePath { get; }

        void DownloadHtml(string uri);
		void DownloadHtml(string uri, string folderPath);

		//Получает весь текст из файла
		string GetString();
		string GetString (string filePath);

		string GetVisibleText(string html);
		string[] SplitToWords(string text);
		IEnumerable<CountedWords> CountUpWord(string[] text);
	}
}
