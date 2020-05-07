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

namespace YsBuildRunner.Build
{
	/// <summary>
	/// Interaction logic for BuildWindow.xaml
	/// </summary>
	public partial class BuildWindow : Window
	{
		private readonly Model model_;

		public BuildWindow(YsBuildRunner.Data.Build build)
		{
			InitializeComponent();

			model_ = new Model(this, build);

			DataContext = model_;
		}

		/// <summary>
		/// Handles list box mouse double click event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">Event data</param>
		private void ListBoxSolutionsMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (model_.CommandEdit.CanExecute(null))
			{
				model_.CommandEdit.Execute(e);
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
		}

		private void OnIncludeChanged(object sender, RoutedEventArgs e)
		{
			model_.IncludeWasChanged(sender);
		}
	}
}
