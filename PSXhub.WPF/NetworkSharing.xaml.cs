using System.IO;
using System.IO.Pipes;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ICS.Manager;
using NETCONLib;
using Panuon.WPF.UI;
using PSXhub.Application.Model;
using PSXhub.Application.Services;
using PSXhub.WPF.ViewModel;

namespace PSXhub.WPF
{
	/// <summary>
	/// Interaction logic for NetworkSharing.xaml
	/// </summary>
	public partial class NetworkSharing : WindowX
	{
		public AppSettings Settings => SettingsManager.Instance.AppSettings;
		public bool IsShared = false;
		private static Mutex? mutex;
		private MainViewModel _viewModel;

		public NetworkSharing()
		{
			InitializeComponent();
			_viewModel = new MainViewModel();
			this.DataContext = _viewModel;
			GetAdapters();
			CheckSharing();
			SelectedAdapters();
		}

		public static bool IsAdminWindowRunning()
		{
			bool createdNew;
			mutex = new Mutex(true, "MyAdminWindowMutex", out createdNew);
			if (!createdNew)
			{
				mutex = null;
			}
			return !createdNew;
		}

		private void SelectedAdapters()
		{
			if (!string.IsNullOrEmpty(Settings.SharerAdapter))
			{
				foreach (ConnectionItem item in NetworkInterfaces_ComboBox.Items)
				{
					if (item.Nic.Id == Settings.SharerAdapter)
					{
						NetworkInterfaces_ComboBox.SelectedItem = item;
						break;
					}
				}
			}

			if (!string.IsNullOrEmpty(Settings.RecipientAdapter))
			{
				foreach (ConnectionItem item in ActiveNetworkInterfaces_ComboBox.Items)
				{
					if (item.Nic.Id == Settings.RecipientAdapter)
					{
						ActiveNetworkInterfaces_ComboBox.SelectedItem = item;
						break;
					}
				}
			}
		}

		private void GetAdapters()
		{
			var adapters = IcsManager.GetAllIPv4Interfaces()
				.Where(w => w.OperationalStatus == OperationalStatus.Up && w.NetworkInterfaceType != NetworkInterfaceType.Loopback);
			NetworkInterfaces_ComboBox.Items.Clear();
			ActiveNetworkInterfaces_ComboBox.Items.Clear();
			foreach (NetworkInterface adapter in adapters)
			{
				var nic = new ConnectionItem(adapter);
				NetworkInterfaces_ComboBox.Items.Add(nic);
				ActiveNetworkInterfaces_ComboBox.Items.Add(nic);

				var netShareConnection = nic.Connection;
				if (netShareConnection != null)
				{
					var sc = IcsManager.GetConfiguration(netShareConnection);
					if (sc.SharingEnabled)
					{
						IsShared = true;
						CheckSharing();
						switch (sc.SharingConnectionType)
						{
							case tagSHARINGCONNECTIONTYPE.ICSSHARINGTYPE_PUBLIC:
								NetworkInterfaces_ComboBox.SelectedIndex = NetworkInterfaces_ComboBox.Items.Count - 1;
								break;
							case tagSHARINGCONNECTIONTYPE.ICSSHARINGTYPE_PRIVATE:
								ActiveNetworkInterfaces_ComboBox.SelectedIndex = ActiveNetworkInterfaces_ComboBox.Items.Count - 1;
								break;
						}
					}
					else
					{
						IsShared = false;
					}
				}
			}
		}

		private void OpenNetworkConnections_btn_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			BaseService.OpenNetworkConnections();
		}

		private void AdapterChecker(object sender, SelectionChangedEventArgs e)
		{
			if (ActiveNetworkInterfaces_ComboBox.SelectedValue?.ToString() != null && ActiveNetworkInterfaces_ComboBox.SelectedValue?.ToString() != null
				&& ActiveNetworkInterfaces_ComboBox.SelectedValue?.ToString() == NetworkInterfaces_ComboBox.SelectedValue?.ToString())
			{
				NetworkInterfaces_ComboBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f85149"));
				NetworkInterfaces_ComboBox.BorderThickness = new Thickness(1.5);
				ActiveNetworkInterfaces_ComboBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f85149"));
				ActiveNetworkInterfaces_ComboBox.BorderThickness = new Thickness(1.5);

				StartSharing_btn.IsEnabled = false;
				// ToolTip tooltip = new ToolTip
				// {
				// 	Content = "Check your selected adaptors",
				// 	
				// };
				StartBorder.ToolTip = "Check your selected adaptors";
				StartBorder.Visibility = Visibility.Visible;
			}

			else
			{
				NetworkInterfaces_ComboBox.BorderThickness = new Thickness(0);
				ActiveNetworkInterfaces_ComboBox.BorderThickness = new Thickness(0);

				StartSharing_btn.IsEnabled = true;
				StartBorder.ToolTip = null;
				StartBorder.Visibility = Visibility.Collapsed;
			}
		}
		private void ButtonOverlay_MouseEnter(object sender, MouseEventArgs e)
		{
			Mouse.OverrideCursor = Cursors.No;
		}

		private void ButtonOverlay_MouseLeave(object sender, MouseEventArgs e)
		{
			Mouse.OverrideCursor = null;
		}

		private async void StartSharing_btn_Click(object sender, RoutedEventArgs e)
		{
			var internetAdapter = NetworkInterfaces_ComboBox.SelectedItem as ConnectionItem;
			var localAdapter = ActiveNetworkInterfaces_ComboBox.SelectedItem as ConnectionItem;

			if ((internetAdapter == null) || (localAdapter == null))
			{
				MessageBox.Show(@"Please select both connections.");
				return;
			}
			if (internetAdapter.Connection == localAdapter.Connection)
			{
				MessageBox.Show(@"Please select different connections.");
				return;
			}

			try
			{
				LoadStart();
				if (NetworkInterfaces_ComboBox.SelectedItem != null
				&& ActiveNetworkInterfaces_ComboBox.SelectedItem != null)
				{
					var SharerAdapter = NetworkInterfaces_ComboBox.SelectedItem as ConnectionItem;
					var RecipientAdapter = ActiveNetworkInterfaces_ComboBox.SelectedItem as ConnectionItem;
					Settings.SharerAdapter = SharerAdapter.Nic.Id;
					Settings.RecipientAdapter = RecipientAdapter.Nic.Id;
					SettingsManager.Instance.SaveSettings();
				}
				await Task.Run(() =>
				{
					IcsManager.ShareConnection(internetAdapter.Connection, localAdapter.Connection);
				});
				GetAdapters();
				FixedStart();
				ShowMessage("Successful sharing", "Internet sharing was successful.", MessageBoxIcon.Success);
				SendMessageToMainWindow("adapters");
			}
			catch (Exception ex)
			{
				FixedStart();
				//MessageBox.Show(ex.ToString());
				ShowMessage("Sharing failed", "Internet sharing failed.", MessageBoxIcon.Error);
				StartSharing_btn.Visibility = Visibility.Collapsed;
				ResetIcs_btn.Visibility = Visibility.Visible;
			}
		}

		private void CheckSharing()
		{
			if (IsShared)
			{
				StartSharing_btn.Visibility = Visibility.Collapsed;
				StopSharing_btn.Visibility = Visibility.Visible;
				NetworkInterfaces_ComboBox.IsEnabled = false;
				ActiveNetworkInterfaces_ComboBox.IsEnabled = false;
			}
			else
			{
				StartSharing_btn.Visibility = Visibility.Visible;
				StopSharing_btn.Visibility = Visibility.Collapsed;
				NetworkInterfaces_ComboBox.IsEnabled = true;
				ActiveNetworkInterfaces_ComboBox.IsEnabled = true;
			}
		}

		private async void ShowMessage(string sub, string text, MessageBoxIcon type)
		{
			var notic =
			NoticeBox.Show(text, sub, type);
			await Task.Delay(1500);
			notic.Close();
		}

		private void OkNetworkConnections_btn_Click(object sender, RoutedEventArgs e)
		{
			SendMessageToMainWindow("adapters");
			this.Close();
		}

		private void SendMessageToMainWindow(string message)
		{
			try
			{
				using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "MyPipe", PipeDirection.Out))
				{
					pipeClient.Connect();
					using (StreamWriter writer = new StreamWriter(pipeClient))
					{
						writer.WriteLine(message);
						writer.Flush();
					}
				}
			}
			catch (Exception ex)
			{
			}
		}

		private async void StopSharing_btn_Click(object sender, RoutedEventArgs e)
		{
			LoadStop();
			var activeAdapter = ActiveNetworkInterfaces_ComboBox.SelectedItem as ConnectionItem;
			await Task.Run(() =>
			{
				BaseService.StopSharing();
			});
			await Task.Run(() =>
			{
				BaseService.EnableDHCP(activeAdapter?.Nic.Name);
			});
			//BaseService.EnableDHCPForAllInterfaces();
			//IcsManager.ShareConnection(null, null);
			GetAdapters();
			CheckSharing();
			SelectedAdapters();
			FixedStop();
			SendMessageToMainWindow("adapters");
		}

		private void LoadStart()
		{
			Start_btn_Ring.Visibility = Visibility.Visible;
			Start_btn_Text.Visibility = Visibility.Collapsed;
			StartSharing_btn.IsEnabled = false;
		}

		private void FixedStart()
		{
			Start_btn_Ring.Visibility = Visibility.Collapsed;
			Start_btn_Text.Visibility = Visibility.Visible;
			StartSharing_btn.IsEnabled = true;
		}

		private void LoadStop()
		{
			Stop_btn_Ring.Visibility = Visibility.Visible;
			Stop_btn_Text.Visibility = Visibility.Collapsed;
			StopSharing_btn.IsEnabled = false;
		}

		private void FixedStop()
		{
			Stop_btn_Ring.Visibility = Visibility.Collapsed;
			Stop_btn_Text.Visibility = Visibility.Visible;
			StopSharing_btn.IsEnabled = true;
		}
		private void LoadReset()
		{

			Reset_btn_Ring.Visibility = Visibility.Visible;
			Reset_btn_Ring.Visibility = Visibility.Collapsed;
			ResetIcs_btn.IsEnabled = false;
		}

		private void FixedReset()
		{
			Reset_btn_Ring.Visibility = Visibility.Collapsed;
			Reset_btn_Ring.Visibility = Visibility.Visible;
			ResetIcs_btn.IsEnabled = true;
		}

		private async void ResetIcs_btn_Click(object sender, RoutedEventArgs e)
		{
			LoadReset();
			await Task.Run(() =>
			{
				BaseService.RestartICSService();
			});
			FixedReset();
			ResetIcs_btn.Visibility = Visibility.Collapsed;
			StartSharing_btn.Visibility = Visibility.Visible;
			SendMessageToMainWindow("adapters");
		}
	}
}
