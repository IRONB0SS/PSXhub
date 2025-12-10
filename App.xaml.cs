using ICS;
using PSXhub.Application.Services;
using PSXhub.Contracts.Models;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;

namespace PSXhub.NetworkSharing.WPF
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : System.Windows.Application
	{
		public AppSettings Settings => SettingsManager.Instance.AppSettings;
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			Window windowToOpen;

			if (Manager.GetActiveNetworkConnections().Any())
			{
				windowToOpen = new MainWindow();
			}
			else
			{
				windowToOpen = new WarningWindow();
			}

			windowToOpen.Show();
		}

		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);
		private static Mutex mutex = null;

		protected override void OnStartup(StartupEventArgs e)
		{
			var savedLang = Language.GetById(Settings.Language);
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(savedLang.Symbol);
			base.OnStartup(e);

			const string mutexName = "PSXhub Network Sharing";

			bool createdNew;
			mutex = new Mutex(true, mutexName, out createdNew);

			if (!createdNew)
			{
				Process current = Process.GetCurrentProcess();
				foreach (var process in Process.GetProcessesByName(current.ProcessName))
				{
					if (process.Id != current.Id)
					{
						SetForegroundWindow(process.MainWindowHandle);
						break;
					}
				}

				Shutdown();
				return;
			}
		}
	}
}
