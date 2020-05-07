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
using System.Threading;
using YsBuildRunner.Data;

namespace YsBuildRunner.Main
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Constants

		private const string InstanceMutexName = "Integrated Build Mutex";
		#endregion

		#region Fields

		/// <summary>
		/// Holds the mutex.
		/// </summary>
		private Mutex mutex_;

		private Model model_;
		#endregion

		public MainWindow()
		{
			InitializeComponent();

			model_ = new Model(this);

			DataContext = model_;
		}

		#region Methods

		/// <summary>
		/// Handles double click on build row.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">Event dat</param>
		private void ListBoxBuildsMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (listBoxBuilds.SelectedItem as YsBuildRunner.Data.Build != null)
			{
				model_.CommandEdit.Execute(e);
			}
		}

		/// <summary>
		/// Handles menu item About.
		/// </summary>
		/// <param name="sender">the sender.</param>
		/// <param name="e">The event data</param>
		private void MenuItemAboutClick(object sender, RoutedEventArgs e)
		{
			(new AboutWindow()).ShowDialog();
		}

		/// <summary>
		/// Handles event Window Loaded
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				// Check, if another instance is active already.
				mutex_ = Mutex.OpenExisting(InstanceMutexName);
				MessageBox.Show("Another instance of the application is running already.", Title, MessageBoxButton.OK,
								MessageBoxImage.Information);
				mutex_.Close();
				mutex_ = null;

				Close();
			}
			catch (WaitHandleCannotBeOpenedException)
			{
				// No another active instance.
				mutex_ = new Mutex(true, InstanceMutexName);
				WindowManager.Instance.WindowConfig.SetWindowPosition(this);
			}
		}

		/// <summary>
		/// Handles event Window Closing
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = !model_.CanClose;

			if (e.Cancel)
			{
				return;
			}

			if (mutex_ != null)
			{
				// Save window positions.
				WindowManager.Instance.WindowConfig.SaveWindowPosition(this);

				// Save window configuration.
				WindowManager.Instance.Save();

				mutex_.ReleaseMutex();
			}
		}
		#endregion
	}
}
