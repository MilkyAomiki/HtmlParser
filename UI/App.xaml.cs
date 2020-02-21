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
        private HtmlTools htmlTools = new HtmlTools();
        public App()
        {
            w_main.Show();
            w_main.btnToParsePage_Click += OpenParseWindow;

            w_parser.loadHtmlClick += LoadHtml;
            w_parser.openExtractPage += ShowLastFilePath;
            w_parser.showTextFromHtml_Click += ShowTextFromHtml;
            
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
        private string ShowTextFromHtml()
        {
            string html = htmlTools.GetString();
            return htmlTools.GetVisibleText(html);
        }
    }
}
