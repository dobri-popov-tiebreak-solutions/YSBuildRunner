using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using YsBuildRunner.Properties;

namespace YsBuildRunner
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		/// <summary>
		/// Sets the unhandled exception handler and starts the application.
		/// </summary>
		/// <param name="e">Startup arguents.</param>
		protected override void OnStartup(StartupEventArgs e)
		{
			Current.DispatcherUnhandledException += CurrentDispatcherUnhandledException;
			
			if (Settings.Default.UpgradeRequired)
			{
				Settings.Default.Upgrade();
				Settings.Default.UpgradeRequired = false;
				Settings.Default.Save();
				Settings.Default.Reload();
			}

			base.OnStartup(e);
		}

		/// <summary>
		/// Handles the unhandled exception.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The data.</param>
		private static void CurrentDispatcherUnhandledException(object sender,
			System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			var errorWindow = new ErrorWindow();
			errorWindow.errorText.Text = e.Exception.ToString();
			errorWindow.ShowDialog();

			e.Handled = true;
			e.Dispatcher.InvokeShutdown();
		}
	}
}
