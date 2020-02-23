using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using  Newtonsoft.Json;

namespace UI
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        public Action BtnToParsePage_Click;
        public Func<string> DefaultFolderGet;
        public Action<string> SetCustomFolder;
        public Func<string> GetCustomFolder;
		private string CustomFolder {  set => SetCustomFolder.Invoke(value); get => GetCustomFolder.Invoke(); }

        private string DefaultFolder => DefaultFolderGet.Invoke();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Btn_settings_Click(object sender, RoutedEventArgs e)
		{
			txtBx_Path.Text = Directory.Exists( CustomFolder) ? CustomFolder : DefaultFolder;

			tabItem_settings.IsSelected = true;
		}

		private void Btn_toParsePage_Click(object sender, RoutedEventArgs e)
		{
            BtnToParsePage_Click.Invoke();
		}

		private void Btn_Exit_Click(object sender, RoutedEventArgs e)
		{
			App.Current.Shutdown();
		}

		private void Btn_exitSettings_Click(object sender, RoutedEventArgs e)
		{
			if (txtBx_Path.Text != DefaultFolder && txtBx_Path.Text != CustomFolder)
			{
				if (Directory.Exists(txtBx_Path.Text))
				{
					CustomFolder = txtBx_Path.Text;
					using (StreamWriter file = File.CreateText("settings.json"))
					{
						JsonSerializer serializer = new JsonSerializer();
						serializer.Serialize(file, CustomFolder);
					}
				}
			}else if (txtBx_Path.Text == DefaultFolder && CustomFolder!= null)
			{
				CustomFolder = null;
				File.Delete("settings.json");

			}
			tabItem_main.IsSelected = true;
		}

		private void btn_choosePath_Click(object sender, RoutedEventArgs e)
		{
			using (FolderBrowserDialog folderBrowser = new FolderBrowserDialog())
			{
				
				folderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
				if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					txtBx_Path.Text = folderBrowser.SelectedPath;
				}
			}
		}
	}
}
