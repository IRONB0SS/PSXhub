using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace PSXhub.WPF
{
    /// <summary>
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();
        }

		private void OpenGithub(object sender, RoutedEventArgs e)
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = "https://github.com/IRONB0SS/PSXhub",
				UseShellExecute = true
			});
		}
	}
}
