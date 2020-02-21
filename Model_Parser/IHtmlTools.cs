namespace Model_Parser
{
    public interface IHtmlTools
	{
        string DefaultDirectory { get; }
        string CustomDirectory { get; }
        string HtmlFilePath { get; }

        void DownloadHtml(string uri);
		void DownloadHtml(string uri, string folderPath);
		string GetString();
		string GetString (string filePath);
		string GetVisibleText(string html);
		string[] SplitToWords(string text);
		int CountUpWord(string[] text, string word);
	}
}
