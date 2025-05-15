using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PSXhub.Application.Model;
using PSXhub.Application.Server;
using PSXhub.Application.Services;
using PSXhub.WPF.ViewModel;

namespace PSXhub.WPF
{
	public partial class MainWindow : Window
	{
		public AppSettings Settings => SettingsManager.Instance.AppSettings;
		private readonly Main main = Main.Instance();
		private MainViewModel _viewModel;
		private HttpListenerHelp _server;
		private bool Is = true;
		public MainWindow()
		{
			InitializeComponent();
			_viewModel = new MainViewModel();
			this.DataContext = _viewModel;
			GetServerIp();
			IpChecker();
			Task.Run(() => StartPipeServer());
		}

		private void Minimize_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		private void Maximize_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
		}

		private void Close_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				this.DragMove();
			}
		}

		private void StartPipeServer()
		{
			while (true)
			{
				try
				{
					using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("MyPipe", PipeDirection.In))
					{
						pipeServer.WaitForConnection();
						using (StreamReader reader = new StreamReader(pipeServer))
						{
							string message = reader.ReadLine();
							Dispatcher.Invoke(() => UpdateNetwork(message));
						}
					}
				}
				catch (Exception ex)
				{
				}
			}
		}

		private void UpdateNetwork(string message)
		{
			if (message == "adapters")
			{
				IpChecker();
				GetServerIp();
			}
		}

		private void GetServerIp()
		{
			var ipList = BaseService.GetIPs();
			IPs.Items.Clear();
			foreach (var i in ipList)
			{
				IPs.Items.Add(i);
			}

			// IPs.Items.Add("192.168.137.1");
            IPs.SelectedIndex = 0;
		}

		private void Start_MouseEnter(object sender, MouseEventArgs e)
		{
			this.Cursor = Cursors.Hand;
		}

		private void Start_MouseLeave(object sender, MouseEventArgs e)
		{
			this.Cursor = Cursors.Arrow;
		}

		private void WindowX_Loaded(object sender, RoutedEventArgs e)
		{
			
		}

		private void WindowX_Closing(object? sender, CancelEventArgs e)
		{
			
		}

		private void WindowX_Closed(object? sender, EventArgs e)
		{
			if (Is)
			{
				System.Windows.Application.Current.Shutdown();
			}
		}

		private bool IsUserAdministrator()
		{
			var identity = WindowsIdentity.GetCurrent();
			var principal = new WindowsPrincipal(identity);
			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}

		private void OpenNetworkSharing_btn_Click(object sender, RoutedEventArgs e)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				FileName = "Psxhub.exe",
				Arguments = "adminWindow",
				Verb = "runas",
				UseShellExecute = true,
			};

			try
			{
				Process.Start(startInfo);
				Settings.IsLocalAdmin = true;
				Settings.IsAdminNetworkSharingWindow = true;
				SettingsManager.Instance.SaveSettings();
			}
			catch
			{
			}
		}

		private void StartButton_MouseEnter(object sender, MouseEventArgs e)
		{
			this.Cursor = Cursors.Hand;
		}

		private void StartButton_MouseLeave(object sender, MouseEventArgs e)
		{
			this.Cursor = Cursors.Arrow;
		}

		private void RefreshIps(object sender, EventArgs e)
		{
			GetServerIp();
		}

		private void IpChecker()
		{
			bool isIp = false;

			foreach (var ip in BaseService.GetIPs())
			{
				if (ip.ToString() == "192.168.137.1")
				{
					isIp = true;
					break;
				}
			}

			bool show = false;

			if (!BaseService.CheckICSPresent() && isIp)
			{
				show = true;
			}

			if (Settings.HideNetIpAlert && !show)
			{
				Network_War_Icon.Visibility = Visibility.Collapsed;
			}
			else
			{
				Network_War_Icon.Visibility = Visibility.Visible;
			}
		}

        private void IpsChange(object sender, SelectionChangedEventArgs e)
        {
			IpChecker();
        }

		private void HideNetAlert_Click(object sender, RoutedEventArgs e)
		{
			Network_btn_PopUp.IsOpen = false;
			Settings.HideNetIpAlert = true;
			SettingsManager.Instance.SaveSettings();
			IpChecker();
		}

		private void Open_Site(object sender, RoutedEventArgs e)
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = "https://bazibaazar.ir",
				UseShellExecute = true
			});
		}

		private void btnStart_Click(object sender, RoutedEventArgs e)
		{
			Is = false;
			var port = int.Parse(Port.Text);
			var ip = IPs.SelectedItem as IPAddress;

			Task.Run(() =>
			{
				_server = new HttpListenerHelp(ip, port);
				_server.Start();
			});

			main.GetServer($"{ip} : {port}");
			this.Close();
			main.Show();
		}

		private void Network_War_Icon_OnMouseEnter(object sender, MouseEventArgs e)
		{
			Network_btn_PopUp.IsOpen = true;
		}

		private async void Network_War_Icon_OnMouseLeave(object sender, MouseEventArgs e)
		{
			Network_btn_PopUp.IsOpen = false;
		}
	}
}