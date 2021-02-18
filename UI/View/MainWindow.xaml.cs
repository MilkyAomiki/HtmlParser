using System;
using System.IO;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Forms;
using Newtonsoft.Json;
using MessageBox = System.Windows.MessageBox;

namespace UI
{
	/// <summary>
	/// Логика взаимодействия для главного окна MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Events

		public Action BtnToParsePage_Click;
		public Func<string> GetDefaultFolder;
		public Action<string> SetCustomFolder;
		public Func<string> GetCustomFolder;

		#endregion

		//Значения устанавливаются из App.cs
		private string CustomFolder { set => SetCustomFolder.Invoke(value); get => GetCustomFolder.Invoke(); }
		private string DefaultFolder => GetDefaultFolder.Invoke();

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
					//Проверяет возможность записи в выбранную директорию
					try
					{
						using (FileStream fs = new FileStream(txtBx_Path.Text + @"\test.html", FileMode.Create, FileAccess.Write))
						{
							fs.WriteByte(0xff);
						}

						if (File.Exists(txtBx_Path.Text + @"\test.html"))
						{
							File.Delete(txtBx_Path.Text + @"\test.html");
						}
					}
					catch (Exception)
					{
						MessageBox.Show("Cannot access directory", txtBx_Path.Text, MessageBoxButton.OK);
						return;
					}

					CustomFolder = txtBx_Path.Text;

					using (StreamWriter file = File.CreateText("settings.json"))
					{
						JsonSerializer serializer = new JsonSerializer();
						serializer.Serialize(file, CustomFolder);
					}
				}
			}
			else if (txtBx_Path.Text == DefaultFolder && CustomFolder!= null)
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
