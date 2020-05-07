using System;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;

namespace YsBuildRunner
{
	/// <summary>
	/// Interaction logic for AboutWindow.xaml
	/// </summary>
	public partial class AboutWindow : Window
	{

		#region Properties

		public static string AssemblyTitle
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (attributes.Length > 0)
				{
					var titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (0 != titleAttribute.Title.Length)
					{
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public static string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public static string AssemblyDescription
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public static string AssemblyProduct
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public static string AssemblyCopyright
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public static string AssemblyCompany
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}
		#endregion Properties

		#region Methods

		public AboutWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Title = String.Format("About {0}", AssemblyTitle);
			textBlockProductName.Text = AssemblyProduct;
			textBlockVersion.Text = String.Format("Version {0}", AssemblyVersion);
			textBlockCopyright.Text = AssemblyCopyright;
			textBlockCompanyName.Text = AssemblyCompany;
			textBlockDescription.Text = AssemblyDescription;
		}

		#endregion Methods

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void HyperlinkClick(object sender, RoutedEventArgs e)
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
