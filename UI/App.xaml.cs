using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Model_Parser;
using  Newtonsoft.Json;

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
	        DeserializeSettings();

            w_main.SetCustomFolder      += SetCustomFolder;
            w_main.GetCustomFolder      += () => htmlTools.CustomDirectory;
            w_main.DefaultFolderGet     += () => htmlTools.DefaultDirectory;
            w_main.BtnToParsePage_Click += OpenParseWindowAppear;
            w_main.Show();

            w_parser.DefaultDirectory = htmlTools.DefaultDirectory;
            w_parser.CustomDirectory  = htmlTools.CustomDirectory;

            w_parser.LoadHtmlClick          += LoadHtml;
            w_parser.OpenExtractPage        += ShowLastFilePath;
            w_parser.ShowTextFromHtml_Click += ShowTextFromHtml;
            w_parser.SplitToWords           += SplitToWords;
            w_parser.CountUpWord            += CountUpWords;
        }

        private void DeserializeSettings()
        {
	        if (File.Exists("settings.json"))
	        {
		        using (StreamReader file = File.OpenText("settings.json"))
		        {
                    JsonSerializer serializer = new JsonSerializer();
                    htmlTools.CustomDirectory = (string)serializer.Deserialize(file, typeof(string));
		        }
            }

        }

        private void SetCustomFolder(string path)
        {
	        htmlTools.CustomDirectory = path;
	        w_parser.CustomDirectory = htmlTools.CustomDirectory;
        }

        private void OpenParseWindowAppear()
        {
            w_main.Hide();
            w_parser.Closing += (sender, args) =>
            {
	            args.Cancel = true;
	            w_parser.Hide();
	            w_main.Show();
            };
            w_parser.Owner = w_main;
            w_parser.Show();
        }

        private void LoadHtml(string html)
        {
	        try
	        {
		        htmlTools.DownloadHtml(html);
            }
	        catch (Exception e)
	        {
		        Console.WriteLine(e);
	        }
            
        }
        
        private string ShowLastFilePath()
        {
            return htmlTools.HtmlFilePath;
        }
        private string ShowTextFromHtml(string path)
        {
            Thread.Sleep(4000);
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
