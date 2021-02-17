using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Parser;
using Newtonsoft.Json;

namespace UI
{
	/// <summary>
	/// Связывет HtmlTools и View, управляет окнами
	/// </summary>
	public partial class App : Application
	{
		private readonly MainWindow w_main = new MainWindow();
		private readonly ParserMain w_parser = new ParserMain();
		private readonly IHtmlTools htmlTools = new HtmlTools();

		public App()
		{
			DeserializeSettings();

			#region View Events

			w_main.SetCustomFolder += SetCustomFolder;
			w_main.GetCustomFolder += () => htmlTools.CustomDirectory;
			w_main.GetDefaultFolder += () => htmlTools.DefaultDirectory;
			w_main.BtnToParsePage_Click += OpenParseWindowAppear;
			w_main.Show();

			w_parser.DefaultDirectory = htmlTools.DefaultDirectory;
			w_parser.CustomDirectory = htmlTools.CustomDirectory;

			w_parser.LoadHtmlClick += LoadHtml;
			w_parser.OpenExtractPage += ShowLastFilePath;
			w_parser.ShowTextFromHtml_Click += ShowTextFromHtml;
			w_parser.SplitToWords += SplitToWords;
			w_parser.CountUpWord += CountUpWords;


			#endregion
		}

		private void DeserializeSettings()
		{
			if (File.Exists("settings.json"))
			{
				using StreamReader file = File.OpenText("settings.json");
				JsonSerializer serializer = new JsonSerializer();
				htmlTools.CustomDirectory = (string)serializer.Deserialize(file, typeof(string));
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
			string visibleText;
			try
			{
				string html = htmlTools.GetText(path);
				visibleText = htmlTools.GetVisibleText(html);

			}
			catch (FileFormatException)
			{
				visibleText = "Wrong file extension";
			}
			catch (InsufficientMemoryException)
			{
				visibleText = "File is too large";
			}
			catch (FileNotFoundException)
			{
				visibleText = "File not found";

			}

			return visibleText;
		}

		private string[] SplitToWords(string text)
		{
			return htmlTools.SplitToWords(text);
		}

		private IEnumerable<CountedWords> CountUpWords(string[] words)
		{
			return htmlTools.CountUpWords(words);
		}
	}
}
