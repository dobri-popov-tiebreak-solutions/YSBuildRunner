using System.Windows;
using System.Windows.Documents;

namespace YsBuildRunner
{
	/// <summary>
	/// Interaction logic for ErrorWindow.xaml
	/// </summary>
	public partial class ErrorWindow : Window
	{
		public ErrorWindow()
		{
			InitializeComponent();
		}
		private void HyperlinkToSiteClick(object sender, RoutedEventArgs e)
		{
			var link = sender as Hyperlink;

			if (link != null)
			{
				var p = new System.Diagnostics.Process();
				p.StartInfo.FileName = link.NavigateUri.ToString();
				p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
				p.Start();
			}

			e.Handled = true;
		}
	}
}
