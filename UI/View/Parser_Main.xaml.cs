using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace UI
{
	/// <summary>
	/// Логика взаимодействия для Parser_Main.xaml
	/// </summary>
	public partial class Parser_Main : Window
	{
        public Action<string> LoadHtmlClick;
        public Func<string> OpenExtractPage;
        public Func<string, string[]> SplitToWords;
        public Func<string[], string, int> CountUpWord;
        public Func<string, string> ShowTextFromHtml_Click;

        public string DefaultDirectory;
        public string CustomDirectory;

        public Parser_Main()
		{
			InitializeComponent();
            item_load.IsSelected = true;
        }

        private void Btn_loadHtml_Click(object sender, RoutedEventArgs e)
		{
            LoadHtmlClick.Invoke(txtBx_html.Text);
		}

        private void Btn_goToExtractPage_Click(object sender, RoutedEventArgs e)
        {
            txtBx_pathToFile.Text = OpenExtractPage.Invoke();
            item_extractText.IsSelected = true;
        }

        private void Btn_goToStat_Click(object sender, RoutedEventArgs e)
        {
            string[] words = SplitToWords.Invoke(lbl_htmlVisibleText.Text);


            var sortedWords = words.GroupBy(x => x).Select(g => new { Name = g.Key, Count = g.Count() });
            foreach (var item in words)
            {
                lbl_wordsStats.Content += item + " - " + CountUpWord(words, item).ToString() + "\n";
            }
            item_statistic.IsSelected = true;
        }

        private void Btn_extractVisibleText_Click(object sender, RoutedEventArgs e)
        {
            lbl_htmlVisibleText.Text = ShowTextFromHtml_Click.Invoke(txtBx_pathToFile.Text);
        }

        private void Btn_openFileExplorer_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Html files (*.html)|*.html";

            if (Directory.Exists(DefaultDirectory))
            {
                fileDialog.InitialDirectory = DefaultDirectory;
            }
            else if (Directory.Exists(CustomDirectory))
            {
                fileDialog.InitialDirectory = CustomDirectory;
            }

            fileDialog.FileOk += SetPath;
            fileDialog.ShowDialog();

            void SetPath(object obj, CancelEventArgs em) => txtBx_pathToFile.Text = fileDialog.FileName;
        }
    }
}
