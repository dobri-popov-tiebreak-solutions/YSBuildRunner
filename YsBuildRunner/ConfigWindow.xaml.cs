using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SWF = System.Windows.Forms;
using IO = System.IO;

namespace YsBuildRunner
{
	/// <summary>
	/// Interaction logic for ConfigWindow.xaml
	/// </summary>
	public partial class ConfigWindow : Window
	{
		public string PathToMSBuild
		{
			get;
			private set;
		}

		public ConfigWindow(string pathToMSBuild)
		{
			InitializeComponent();

			PathToMSBuild = pathToMSBuild;

			textBoxPathToMSBuild.Text = PathToMSBuild;
		}

		private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
		{
			using (var openFileDialog = new SWF.OpenFileDialog
			{
				DefaultExt = ".sln",
				Filter = "MSBuild files (MSBuild.exe)|MSBuild.exe",
				Title = "Select MSBuild file"
			})
			{

				if (openFileDialog.ShowDialog() == SWF.DialogResult.OK)
				{
					var file = openFileDialog.FileName;
					file = IO.Path.GetFileNameWithoutExtension(file);
					PathToMSBuild = file;
				}
			}
		}

		private void ButtonOK_Click(object sender, RoutedEventArgs e)
		{
			PathToMSBuild = textBoxPathToMSBuild.Text;
			DialogResult = true;
			Close();
		}

		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
}
