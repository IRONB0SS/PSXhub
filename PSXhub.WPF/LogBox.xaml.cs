using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PSXhub.Application.Services;

namespace PSXhub.WPF
{
	/// <summary>
	/// Interaction logic for LogBox.xaml
	/// </summary>
	public partial class LogBox : UserControl
	{
		public LogBox()
		{
			InitializeComponent();
		}

		private async void Copy_Click(object sender, RoutedEventArgs e)
		{
			Button btn = sender as Button;
			if (btn?.Tag != null)
			{
				Clipboard.SetText(btn.Tag.ToString());
			}

			CopiedPopUp.IsOpen = true;
			CopyIconPath.Data = Geometry.Parse("M13.78 4.22a.75.75 0 0 1 0 1.06l-7.25 7.25a.75.75 0 0 1-1.06 0L2.22 9.28a.751.751 0 0 1 .018-1.042.751.751 0 0 1 1.042-.018L6 10.94l6.72-6.72a.75.75 0 0 1 1.06 0Z");
			CopyIconPath.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3FB950"));
			btn!.IsEnabled = false;

			await Task.Delay(2000);

			CopiedPopUp.IsOpen = false;
			CopyIconPath.Data = Geometry.Parse("M0 6.75C0 5.784.784 5 1.75 5h1.5a.75.75 0 0 1 0 1.5h-1.5a.25.25 0 0 0-.25.25v7.5c0 .138.112.25.25.25h7.5a.25.25 0 0 0 .25-.25v-1.5a.75.75 0 0 1 1.5 0v1.5A1.75 1.75 0 0 1 9.25 16h-7.5A1.75 1.75 0 0 1 0 14.25Z M5 1.75C5 .784 5.784 0 6.75 0h7.5C15.216 0 16 .784 16 1.75v7.5A1.75 1.75 0 0 1 14.25 11h-7.5A1.75 1.75 0 0 1 5 9.25Zm1.75-.25a.25.25 0 0 0-.25.25v7.5c0 .138.112.25.25.25h7.5a.25.25 0 0 0 .25-.25v-7.5a.25.25 0 0 0-.25-.25Z");
			CopyIconPath.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9198A1"));
			btn!.IsEnabled = true;
		}

		private async void Download_Click(object sender, RoutedEventArgs e)
		{
			Button btn = sender as Button;
			if (btn?.Tag != null)
			{
				if (IdmManagerService.IsIDMInstalled(out string idmPath))
				{
					IdmManagerService.OpenIdmWithLink(idmPath, btn.Tag.ToString()!);
				}
				else
				{
					Process.Start(new ProcessStartInfo
					{
						FileName = btn.Tag.ToString(),
						UseShellExecute = true
					});
				}
			}

			DownloadedPopUp.IsOpen = true;
			DownloadIconPath.Data = Geometry.Parse("M13.78 4.22a.75.75 0 0 1 0 1.06l-7.25 7.25a.75.75 0 0 1-1.06 0L2.22 9.28a.751.751 0 0 1 .018-1.042.751.751 0 0 1 1.042-.018L6 10.94l6.72-6.72a.75.75 0 0 1 1.06 0Z");
			DownloadIconPath.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3FB950"));

			await Task.Delay(2000);

			DownloadedPopUp.IsOpen = false;
			DownloadIconPath.Data = Geometry.Parse("M7.25 7.689V2a.75.75 0 0 1 1.5 0v5.689l1.97-1.969a.749.749 0 1 1 1.06 1.06l-3.25 3.25a.749.749 0 0 1-1.06 0L4.22 6.78a.749.749 0 1 1 1.06-1.06l1.97 1.969Z M2.75 14A1.75 1.75 0 0 1 1 12.25v-2.5a.75.75 0 0 1 1.5 0v2.5c0 .138.112.25.25.25h10.5a.25.25 0 0 0 .25-.25v-2.5a.75.75 0 0 1 1.5 0v2.5A1.75 1.75 0 0 1 13.25 14Z");
			DownloadIconPath.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9198A1")); ;
		}

		private void Orbis_Click(object sender, RoutedEventArgs e)
		{
			Button btn = sender as Button;
			if (btn?.Tag != null)
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = btn.Tag.ToString(),
					UseShellExecute = true
				});
			}
		}
	}
}
