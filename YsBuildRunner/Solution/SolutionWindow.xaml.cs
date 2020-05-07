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

namespace YsBuildRunner.Solution
{
	/// <summary>
	/// Interaction logic for SolutionWindow.xaml
	/// </summary>
	public partial class SolutionWindow : Window
	{
		private readonly Model model_;

		public SolutionWindow(Data.Solution solution)
		{
			InitializeComponent();

			model_ = new Model(solution);

			DataContext = model_;
		}

		private void ButtonOkClick(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}
	}
}
