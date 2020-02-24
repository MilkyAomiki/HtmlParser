using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using Parser;

namespace UI
{
	/// <summary>
	/// Логика взаимодействия для окна ParserMain.xaml
	/// </summary>
	public partial class ParserMain : Window
	{
		#region Events

		public Action<string> LoadHtmlClick;
		public Func<string> OpenExtractPage;
		public Func<string, string[]> SplitToWords;
		public Func<string[], IEnumerable<CountedWords>> CountUpWord;
		public Func<string, string> ShowTextFromHtml_Click;

		#endregion

		//Значения устанавливает App.cs
		public string DefaultDirectory;
        public string CustomDirectory;

        public ParserMain()
		{
			InitializeComponent();
            item_load.IsSelected = true;
        }

 #region Load Page

        private void Btn_loadHtml_Click(object sender, RoutedEventArgs e)
        {
	        string html = txtBx_html.Text;
	       
	        BackgroundWorker loadPage = new BackgroundWorker();
            loadPage.DoWork += (object p, DoWorkEventArgs ev) =>
            {
	            LoadHtmlClick.Invoke(html);
            };

            loadPage.RunWorkerCompleted += (object p, RunWorkerCompletedEventArgs ev) => progressBar_loadHtml.IsIndeterminate = false;
            progressBar_loadHtml.IsIndeterminate = true;
	        loadPage.RunWorkerAsync();
            
        }

        private void Btn_goToExtractPage_Click(object sender, RoutedEventArgs e)
        {
            txtBx_pathToFile.Text = OpenExtractPage.Invoke();
            item_extractText.IsSelected = true;
        }

#endregion

 #region Extract Page

        private void Btn_extractVisibleText_Click(object sender, RoutedEventArgs e)
        {
	        BackgroundWorker extractWorker = new BackgroundWorker();
	        string textToExtract = txtBx_pathToFile.Text;
            string extractedText = null;

	        extractWorker.RunWorkerCompleted += (o, args) =>
	        {
		        lbl_htmlVisibleText.Text = extractedText;
                progressBar_extract.IsIndeterminate = false;
	        };
	        extractWorker.DoWork += (o, args) =>
		        extractedText = ShowTextFromHtml_Click.Invoke(textToExtract);
            progressBar_extract.IsIndeterminate = true;
            extractWorker.RunWorkerAsync();
        }

        private void Btn_openFileExplorer_Click(object sender, RoutedEventArgs e)
        {
	        OpenFileDialog fileDialog = new OpenFileDialog();
	        fileDialog.Filter = "Html files (*.html)|*.html";

	        if (Directory.Exists(CustomDirectory))
	        {
                fileDialog.InitialDirectory = CustomDirectory;
	        }
	        else if (Directory.Exists(DefaultDirectory))
	        {
                fileDialog.InitialDirectory = DefaultDirectory;
	        }

	        fileDialog.FileOk += SetPath;
	        fileDialog.ShowDialog();

	        void SetPath(object obj, CancelEventArgs em) => txtBx_pathToFile.Text = fileDialog.FileName;
        }

        private void Btn_goToStat_Click(object sender, RoutedEventArgs e)
        { 
	        string[] words = SplitToWords.Invoke(lbl_htmlVisibleText.Text);
	        var wordsStat = CountUpWord.Invoke(words);
	        wordsStat = wordsStat.OrderByDescending(g => g.Count);

	        if (lbl_wordsStats.Content != null) lbl_wordsStats.Content = null;
            foreach (var item in wordsStat)
	        {
		        lbl_wordsStats.Content += $"{item.Word} - {item.Count} \n";
	        }
	        item_statistic.IsSelected = true;
        }

#endregion
    }
}
