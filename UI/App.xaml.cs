using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Model_Parser;

namespace UI
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{
        private MainWindow w_main = new MainWindow();
        private Parser_Main w_parser = new Parser_Main();
        private IHtmlTools htmlTools = new HtmlTools();

        public App()
        {
            w_main.Show();
            w_main.btnToParsePage_Click += OpenParseWindow;

            w_parser.DefaultDirectory         = htmlTools.DefaultDirectory;
            w_parser.CustomDirectory          = htmlTools.CustomDirectory;

            w_parser.LoadHtmlClick           += LoadHtml;
            w_parser.OpenExtractPage         += ShowLastFilePath;
            w_parser.ShowTextFromHtml_Click  += ShowTextFromHtml;
            w_parser.SplitToWords            += SplitToWords;
            w_parser.CountUpWord             += CountUpWords;
        }

        private void OpenParseWindow()
        {

            w_main.Hide();
            w_parser.Show();
        }

        private void LoadHtml(string html)
        {
            htmlTools.DownloadHtml(html);
        }
        
        private string ShowLastFilePath()
        {
            return htmlTools.HtmlFilePath;
        }
        private string ShowTextFromHtml(string path)
        {
            string html = htmlTools.GetString(path);
            return htmlTools.GetVisibleText(html);
        }
        private string[] SplitToWords(string text)
        {
            return htmlTools.SplitToWords(text);
        }
        private int CountUpWords(string[] words,string word)
        {
            return htmlTools.CountUpWord(words, word);
        }
    }
}
