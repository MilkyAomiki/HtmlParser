using System;
using System.Collections.Generic;
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

namespace UI
{
	/// <summary>
	/// Логика взаимодействия для Parser_Main.xaml
	/// </summary>
	public partial class Parser_Main : Window
	{
        public Action<string> loadHtmlClick;
        public Func<string> openExtractPage;
        public Func<string> showTextFromHtml_Click; 
        public Parser_Main()
		{
			InitializeComponent();
            item_load.IsSelected = true;
            item_extractText.GotFocus += Item_extractText_GotFocus;

        }

        private void Item_extractText_GotFocus(object sender, RoutedEventArgs e)
        {
           txtBx_pathToFile.Text = openExtractPage.Invoke();
        }

        private void btn_loadHtml_Click(object sender, RoutedEventArgs e)
		{
            loadHtmlClick.Invoke(txtBx_html.Text);
		}

        private void btn_goToExtratPage_Click(object sender, RoutedEventArgs e)
        {
            item_extractText.IsSelected = true;
        }

        private void btn_goToStat_Click(object sender, RoutedEventArgs e)
        {
            item_statistic.IsSelected = true;
        }

        private void btn_extractVisibleText_Click(object sender, RoutedEventArgs e)
        {
            lbl_htmlVisibleText.Text = showTextFromHtml_Click.Invoke();
        }
    }
}
