using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;
using PSXhub.Application.Model;
using PSXhub.Application.Services;
using PSXhub.WPF.ViewModel;

namespace PSXhub.WPF
{
	public partial class Main : Window
	{
		public AppSettings Settings => SettingsManager.Instance.AppSettings;
		private static Main _instance;
		private static readonly object SyncObj = new object();
		private bool checkLog = false;
		private MainViewModel _viewModel;
		private ObservableCollection<LogModel> Log { get; set; }
		private readonly HttpClient _httpClient = new HttpClient();
		private ObservableCollection<SearchItem> _filteredItems = new ObservableCollection<SearchItem>();
		private CancellationTokenSource _cancellationTokenSource;
		public Main()
		{
			InitializeComponent();
			SetAutoFinderPath();

			_viewModel = new MainViewModel();
			this.DataContext = _viewModel;
			LogService.LogAdded += AddLogs;
			SearchResultList.ItemsSource = _filteredItems;
		}

		#region max

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			var handle = (new WindowInteropHelper(this)).Handle;
			HwndSource.FromHwnd(handle).AddHook(WindowProc);
		}

		private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			const int WM_GETMINMAXINFO = 0x0024;

			if (msg == WM_GETMINMAXINFO)
			{
				WmGetMinMaxInfo(hwnd, lParam);
				handled = true;
			}

			return IntPtr.Zero;
		}

		private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
		{
			MINMAXINFO mmi = Marshal.PtrToStructure<MINMAXINFO>(lParam);

			IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

			if (monitor != IntPtr.Zero)
			{
				MONITORINFO monitorInfo = new MONITORINFO();
				GetMonitorInfo(monitor, monitorInfo);
				RECT rcWorkArea = monitorInfo.rcWork;
				RECT rcMonitorArea = monitorInfo.rcMonitor;
				mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
				mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
				mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
				mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
			}

			Marshal.StructureToPtr(mmi, lParam, true);
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int x;
			public int y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MINMAXINFO
		{
			public POINT ptReserved;
			public POINT ptMaxSize;
			public POINT ptMaxPosition;
			public POINT ptMinTrackSize;
			public POINT ptMaxTrackSize;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class MONITORINFO
		{
			public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
			public RECT rcMonitor = new RECT();
			public RECT rcWork = new RECT();
			public int dwFlags = 0;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}

		[DllImport("user32.dll")]
		internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

		[DllImport("user32.dll")]
		internal static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);

		private const int MONITOR_DEFAULTTONEAREST = 0x00000002;

		#endregion

		private void Main_OnClosing(object? sender, CancelEventArgs e)
		{
			if (Settings.ActiveTray)
			{
				TaskbarIcon.Visibility = Visibility.Visible;
				e.Cancel = true;
				this.Hide();
			}
			else
			{
				System.Windows.Application.Current.Shutdown();
			}
		}

		public void GetServer(string serverName)
		{
			ServerName.Text = serverName;
		}

		public static Main Instance()
		{
			if (_instance == null)
				lock (SyncObj)
				{
					if (_instance == null)
						_instance = new Main();
				}
			return _instance;
		}

		private void More_Click(object sender, RoutedEventArgs e)
		{
			DropdownMenu.PlacementTarget = Menu;
			DropdownMenu.Placement = PlacementMode.Bottom;
			DropdownMenu.IsOpen = true;
		}

		private void Minimize_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		private void Maximize_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal; //todo
		}

		private void Close_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				if (WindowState == WindowState.Maximized)
				{
					Point mousePosition = PointToScreen(e.GetPosition(this));
					double percentX = mousePosition.X / ActualWidth;
					WindowState = WindowState.Normal;
					Left = mousePosition.X - (ActualWidth * percentX);
					Top = 0;
					DragMove();
				}
				else
				{
					this.DragMove();
				}
			}
		}

		private void SetAutoFinderPath()
		{
			AutoFinderPath.Text = Settings.AutoFinderPath;
		}

		private void AddLogs(LogModel log)
		{
			if (!checkLog)
			{
				Dispatcher.Invoke(() =>
				{
					NoLogPanel.Visibility = Visibility.Collapsed;
					MainGrid.Visibility = Visibility.Visible;
					ClearLogs_btn.IsEnabled = true;
					checkLog = true;
				});
			}

			var basePattern =
				@"^(EP|UP|HP|JP)(\d{4})(-)(CUSA|PPSA)(\d{5})(_00-)(.*[^,_-])(_|-)?(.{1,3}?)(\.)(pkg)$";

			var mandatoryPattern =
				@"^(EP|UP|HP|JP)(\d{4})(-)(CUSA|PPSA)(\d{5})(_00-)(.*[^,_-])(_|-)(.{1,3})(\.)(pkg)$";

			var updatrPattern =
				@"^(EP|UP|HP|JP)(\d{4})(-)(CUSA|PPSA)(\d{5})(_00-)(.*[^,_-])(-)(.)(\d{4})(-)(.*)(_|-)(.{1,3})(\.)(pkg)$";

			Dispatcher.Invoke(() =>
			{
				LogBox newlog = new LogBox();
				if (!string.IsNullOrEmpty(log.Local))
				{
					if (log.LocalNotExist)
					{
						newlog.Local.Text = log.Local;
						newlog.Local.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FA5E55"));
					}
					else
					{
						newlog.Local.Text = BaseService.ShortenText(log.Local, 61);
					}
				}
				newlog.URL.Text = BaseService.ShortenText(log.Url, 100);

				var urlLastSection = log.Url.Split("/").Last();

				Match regexMatch = Regex.Match(urlLastSection, basePattern);

				if (regexMatch.Success)
				{
					newlog.UrlToolTip.Children.Add(
						new TextBlock
						{
							Text = regexMatch.Groups[1].Value ?? string.Empty,
							FontSize = 14,
							FontWeight = FontWeights.Bold,
							Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4493f8"))
						});

					newlog.UrlToolTip.Children.Add(
						new TextBlock
						{
							Text = regexMatch.Groups[2].Value + regexMatch.Groups[3].Value
								   ?? string.Empty,
							FontSize = 14,
							FontWeight = FontWeights.Bold,
							Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1e8ed"))
						});

					newlog.UrlToolTip.Children.Add(
						new TextBlock
						{
							Text = regexMatch.Groups[4].Value + regexMatch.Groups[5].Value ?? string.Empty,
							FontSize = 14,
							FontWeight = FontWeights.Bold,
							Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#db61a2"))
						});

					Match updateMath = Regex.Match(urlLastSection, updatrPattern);

					if (updateMath.Success)
					{
						Match regexMatch2 = Regex.Match(urlLastSection, mandatoryPattern);

						if (regexMatch2.Success)
						{
							// newlog.UrlToolTip.Children.Add(
							// 	new TextBlock
							// 	{
							// 		Text = regexMatch2.Groups[6].Value
							// 			+ regexMatch2.Groups[7].Value
							// 			+ regexMatch2.Groups[8].Value ?? string.Empty,
							// 		FontSize = 14,
							// 		FontWeight = FontWeights.Bold,
							// 		Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1e8ed"))
							// 	});

							newlog.UrlToolTip.Children.Add(
								new TextBlock
								{
									Text = updateMath.Groups[6].Value
										+ updateMath.Groups[7].Value
										+ updateMath.Groups[8].Value
										+ updateMath.Groups[9].Value
										?? string.Empty,
									FontSize = 14,
									FontWeight = FontWeights.Bold,
									Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1e8ed"))
								});

							newlog.UrlToolTip.Children.Add(
								new TextBlock
								{
									Text = updateMath.Groups[10].Value
										   ?? string.Empty,
									FontSize = 14,
									FontWeight = FontWeights.Bold,
									Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f1e05a"))
								});

							newlog.UrlToolTip.Children.Add(
								new TextBlock
								{
									Text = updateMath.Groups[11].Value
										   + updateMath.Groups[12].Value
										   + updateMath.Groups[13].Value
										   ?? string.Empty,
									FontSize = 14,
									FontWeight = FontWeights.Bold,
									Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1e8ed"))
								});

							newlog.UrlToolTip.Children.Add(
								new TextBlock
								{
									Text = updateMath.Groups[14].Value
										   ?? string.Empty,
									FontSize = 14,
									FontWeight = FontWeights.Bold,
									Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#37c386"))
								});
						}

						else
						{
							newlog.UrlToolTip.Children.Add(
								new TextBlock
								{
									Text = updateMath.Groups[6].Value
										   + updateMath.Groups[7].Value
										   + updateMath.Groups[8].Value
										   + updateMath.Groups[9].Value
										   ?? string.Empty,
									FontSize = 14,
									FontWeight = FontWeights.Bold,
									Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1e8ed"))
								});

							newlog.UrlToolTip.Children.Add(
								new TextBlock
								{
									Text = updateMath.Groups[10].Value
										   ?? string.Empty,
									FontSize = 14,
									FontWeight = FontWeights.Bold,
									Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f1e05a"))
								});

							newlog.UrlToolTip.Children.Add(
								new TextBlock
								{
									Text = updateMath.Groups[11].Value
										   + updateMath.Groups[12].Value
										   + updateMath.Groups[13].Value
										   ?? string.Empty,
									FontSize = 14,
									FontWeight = FontWeights.Bold,
									Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1e8ed"))
								});

							newlog.UrlToolTip.Children.Add(
								new TextBlock
								{
									Text = updateMath.Groups[14].Value ?? string.Empty,
									FontSize = 14,
									FontWeight = FontWeights.Bold,
									Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3FB950"))
								});
						}
					}

					else
					{
						Match regexMatch2 = Regex.Match(urlLastSection, mandatoryPattern);

						if (regexMatch2.Success)
						{
							newlog.UrlToolTip.Children.Add(
								new TextBlock
								{
									Text = regexMatch2.Groups[6].Value
										+ regexMatch2.Groups[7].Value
										+ regexMatch2.Groups[8].Value ?? string.Empty,
									FontSize = 14,
									FontWeight = FontWeights.Bold,
									Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1e8ed"))
								});

							newlog.UrlToolTip.Children.Add(
								new TextBlock
								{
									Text = regexMatch2.Groups[9].Value ?? string.Empty,
									FontSize = 14,
									FontWeight = FontWeights.Bold,
									Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#37c386"))
								});
						}

						else
						{
							newlog.UrlToolTip.Children.Add(
								new TextBlock
								{
									Text = regexMatch.Groups[6].Value
										+ regexMatch.Groups[7].Value
										+ regexMatch.Groups[8].Value ?? string.Empty,
									FontSize = 14,
									FontWeight = FontWeights.Bold,
									Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1e8ed"))
								});

							newlog.UrlToolTip.Children.Add(
								new TextBlock
								{
									Text = regexMatch.Groups[9].Value ?? string.Empty,
									FontSize = 14,
									FontWeight = FontWeights.Bold,
									Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1e8ed"))
								});
						}
					}

					newlog.UrlToolTip.Children.Add(
						new TextBlock
						{
							Text = regexMatch.Groups[10].Value + regexMatch.Groups[11].Value ?? string.Empty,
							FontSize = 14,
							FontWeight = FontWeights.Bold,
							Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1e8ed"))
						});
				}

				else
				{
					newlog.UrlToolTip.Children.Add(
						new TextBlock
						{
							Text = urlLastSection ?? string.Empty,
							FontSize = 14,
							FontWeight = FontWeights.Bold,
							Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1e8ed"))
						});
				}

				newlog.Download.Tag = log.Url;
				newlog.Copy.Tag = log.Url;
				newlog.Orbis.Tag = BaseService.LinkTo(log.Url);
				LogPanel.Children.Add(newlog);

				if (LogPanel.Children.Count > 30)
				{
					LogPanel.Children.RemoveAt(0);
				}

				LogPanelScrollViewer.ScrollToBottom();
			});
		}

		private async void ClearLogs_btn_Click(object sender, RoutedEventArgs e)
		{
			LogService.ClearLogs();
			LogPanel.Children.Clear();

			NoLogPanel.Visibility = Visibility.Visible;
			MainGrid.Visibility = Visibility.Collapsed;
			checkLog = false;
			ClearLogs_btn.IsEnabled = false;

			ClearLogsPopUp.IsOpen = true;
			ClearLogsIconPath.Data = Geometry.Parse("M13.78 4.22a.75.75 0 0 1 0 1.06l-7.25 7.25a.75.75 0 0 1-1.06 0L2.22 9.28a.751.751 0 0 1 .018-1.042.751.751 0 0 1 1.042-.018L6 10.94l6.72-6.72a.75.75 0 0 1 1.06 0Z");
			ClearLogsIconPath.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3FB950"));

			await Task.Delay(2000);

			ClearLogsPopUp.IsOpen = false;
			ClearLogsIconPath.Data = Geometry.Parse("M11 1.75V3h2.25a.75.75 0 0 1 0 1.5H2.75a.75.75 0 0 1 0-1.5H5V1.75C5 .784 5.784 0 6.75 0h2.5C10.216 0 11 .784 11 1.75ZM4.496 6.675l.66 6.6a.25.25 0 0 0 .249.225h5.19a.25.25 0 0 0 .249-.225l.66-6.6a.75.75 0 0 1 1.492.149l-.66 6.6A1.748 1.748 0 0 1 10.595 15h-5.19a1.75 1.75 0 0 1-1.741-1.575l-.66-6.6a.75.75 0 1 1 1.492-.15ZM6.5 1.75V3h3V1.75a.25.25 0 0 0-.25-.25h-2.5a.25.25 0 0 0-.25.25Z");
			ClearLogsIconPath.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9198A1"));
		}

		private void AutoFinder_btn_Click(object sender, RoutedEventArgs e)
		{
			if (Settings.FolderQuestion)
			{
				if (Settings.UseOldFolderSelectType)
				{
					string selectedFolder = ShowFolderBrowserDialog(Settings.AutoFinderPath ?? null);
					if (!string.IsNullOrEmpty(selectedFolder))
					{
						Settings.AutoFinderPath = selectedFolder;
						SettingsManager.Instance.SaveSettings();
						SetAutoFinderPath();
					}
				}
				else
				{
					var dialog = new OpenFolderDialog();
					var result = dialog.ShowDialog();

					if (result == true)
					{
						Settings.AutoFinderPath = dialog.FolderName;
						SettingsManager.Instance.SaveSettings();
						SetAutoFinderPath();
					}
				}
			}
			else
			{
				var question = new FolderQuestion();
				question.ShowDialog();
			}
		}

		private string ShowFolderBrowserDialog(string defaultDirectory)
		{
			IntPtr hwnd = new WindowInteropHelper(this).Handle;

			BROWSEINFO browseInfo = new BROWSEINFO
			{
				hwndOwner = hwnd,
				lpszTitle = "لطفاً یک پوشه انتخاب کنید",
				ulFlags = BIF_NEWDIALOGSTYLE | BIF_RETURNONLYFSDIRS,
				lpfn = new BrowseCallbackDelegate(BrowseCallbackMethod),
				lParam = Marshal.StringToHGlobalUni(defaultDirectory)
			};

			IntPtr pidl = SHBrowseForFolder(ref browseInfo);
			if (pidl != IntPtr.Zero)
			{
				char[] path = new char[260];
				SHGetPathFromIDList(pidl, path);
				Marshal.FreeCoTaskMem(pidl);
				return new string(path).TrimEnd('\0');
			}

			return null;
		}
		private delegate int BrowseCallbackDelegate(IntPtr hwnd, int uMsg, IntPtr lParam, IntPtr lpData);
		private static int BrowseCallbackMethod(IntPtr hwnd, int uMsg, IntPtr lParam, IntPtr lpData)
		{
			if (uMsg == BFFM_INITIALIZED)
			{
				SendMessage(hwnd, BFFM_SETSELECTION, 1, lpData);
			}
			return 0;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct BROWSEINFO
		{
			public IntPtr hwndOwner;
			public IntPtr pidlRoot;
			public string lpszTitle;
			public string pszDisplayName;
			public uint ulFlags;
			public BrowseCallbackDelegate lpfn;
			public IntPtr lParam;
			public int iImage;
		}

		private const uint BIF_NEWDIALOGSTYLE = 0x00000040;
		private const uint BIF_RETURNONLYFSDIRS = 0x00000001;
		private const int BFFM_INITIALIZED = 1;
		private const int BFFM_SETSELECTION = 0x400 + 103;

		[DllImport("shell32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SHBrowseForFolder(ref BROWSEINFO lpbi);

		[DllImport("shell32.dll", CharSet = CharSet.Auto)]
		private static extern bool SHGetPathFromIDList(IntPtr pidl, [Out] char[] pszPath);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

		private void LoadScan()
		{
			Scan_btn_Loader.Visibility = Visibility.Visible;
			ScanBtnTxt.Visibility = Visibility.Collapsed;
			ScanBtn.IsEnabled = false;
		}

		private async void FixedScan()
		{
			Scan_btn_Loader.Visibility = Visibility.Collapsed;
			ScanBtnTxt.Visibility = Visibility.Visible;
			ScanBtn.IsEnabled = true;
			ScanFinished.Visibility = Visibility.Visible;
			await Task.Delay(2000);
			ScanFinished.Visibility = Visibility.Collapsed;
		}

		private void ScanBtn_Click(object sender, RoutedEventArgs e)
		{
			if (Settings.ConfirmScanDatabaseActive)
			{
				MessageBoxResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

				if (result == MessageBoxResult.Yes)
				{
					return;
				}
				else
				{
				}
			}
			else
			{
				return;
			}
		}

		private void LocalDatabaseSettingBtn_Click(object sender, RoutedEventArgs e)
		{
			return;
		}

		private void Coffee_OnClick(object sender, RoutedEventArgs e)
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = "https://www.coffeebede.com/psxhub",
				UseShellExecute = true
			});
		}

		private void Credits_OnClick(object sender, RoutedEventArgs e)
		{
			Frame credits = new Frame();
			credits.Navigate(new CreditsPage());
			CustomMessageBox messageBox = new CustomMessageBox("Credits", credits, 440);
			messageBox.ShowDialog();
		}

		private void About_OnClick(object sender, RoutedEventArgs e)
		{
			Frame about = new Frame();
			about.Navigate(new AboutPage());
			CustomMessageBox messageBox = new CustomMessageBox("About the App", about, 440);
			messageBox.ShowDialog();
		}

		private void DonateBTC_OnClick(object sender, RoutedEventArgs e)
		{
			Frame btc = new Frame();
			btc.Navigate(new DonateBitcoinPage());
			CustomMessageBox messageBox = new CustomMessageBox("Donate with Bitcoin", btc);
			messageBox.ShowDialog();
		}

		private void DonateUSDT_OnClick(object sender, RoutedEventArgs e)
		{
			Frame usdt = new Frame();
			usdt.Navigate(new DonateTetherPage());
			CustomMessageBox messageBox = new CustomMessageBox("Donate with Tether", usdt);
			messageBox.ShowDialog();
		}

		private void AppSettings_OnClick(object sender, RoutedEventArgs e)
		{
			Settings settings = new Settings();
			settings.ShowDialog();
		}

		private void TaskbarExit_OnClick(object sender, RoutedEventArgs e)
		{
			System.Windows.Application.Current.Shutdown();
		}

		private void TaskbarIcon_OnTrayLeftMouseDown(object sender, RoutedEventArgs e)
		{
			this.Show();
			this.Activate();
		}

		private void TaskbarIcon_OnTrayRightMouseDown(object sender, RoutedEventArgs e)
		{
			TaskbarIcon.ContextMenu!.Placement = PlacementMode.Top;
			TaskbarIcon.ContextMenu!.IsOpen = true;
		}

		private void Inbox_btn_Click(object sender, RoutedEventArgs e)
		{
			Inbox.IsOpen = true;
		}

		private void Donate_OnClick(object sender, RoutedEventArgs e)
		{
			Frame donate = new Frame();
			donate.Navigate(new DonatePage());
			CustomMessageBox messageBox = new CustomMessageBox("Buy me Coffee", donate, 494.4);
			messageBox.ShowDialog();
		}

		#region Search
		private void Search_btn_Click(object sender, RoutedEventArgs e)
		{
			SearchBoxPopup.IsOpen = true;
		}

		private async void SearchBox_OnKeyUp(object sender, KeyEventArgs e)
		{
			string query = SearchBox.Text.Trim();

			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource = new CancellationTokenSource();
			var token = _cancellationTokenSource.Token;

			try
			{
				await Task.Delay(500, token);
				if (token.IsCancellationRequested) return;


				if (query.Length >= 2)
				{
					List<GameList> results = await SearchApi(query);
					_filteredItems.Clear();

					foreach (var item in results)
					{
						int index = item.Title.ToLower().IndexOf(query.ToLower());
						if (index >= 0)
						{
							// var x = new SearchItem()
							// {
							// 	Image = item.Image,
							// 	BeforeMatch = item.Title.Substring(0, index),
							// 	MatchedText = item.Title.Substring(index, query.Length),
							// 	AfterMatch = item.Title.Substring(index + query.Length),
							// 	GameTitle = item.GameTitle,
							// 	Region = item.Region
							// };
							// _filteredItems.Add(x);
							_filteredItems.Add(new SearchItem()
							{
								Image = item.Image,
								BeforeMatch = item.Title.Substring(0, index),
								MatchedText = item.Title.Substring(index, query.Length),
								AfterMatch = item.Title.Substring(index + query.Length),
								GameTitle = item.GameTitle,
								Region = item.Region
							});
						}
					}
					SearchResultList.ItemsSource = _filteredItems;
					if (results.Count > 0)
					{
						NullSearch.Visibility = Visibility.Collapsed;
						NotFoundSearch.Visibility = Visibility.Collapsed;
						SearchResultList.Visibility = Visibility.Visible;
					}
					else
					{
						NullSearch.Visibility = Visibility.Collapsed;
						SearchResultList.Visibility = Visibility.Collapsed;
						NotFoundSearch.Visibility = Visibility.Visible;
					}
				}
				else
				{
					NullSearch.Visibility = Visibility.Visible;
					SearchResultList.Visibility = Visibility.Collapsed;
				}
			}
			catch (Exception exception)
			{
			}
		}

		private async Task<List<GameList>> SearchApi(string query)
		{
			try
			{
				string url = $"https://psxhub.bazibaazar.ir/api/api/Game/Search/{query}";
				string json = await _httpClient.GetStringAsync(url);
				return JsonSerializer.Deserialize<List<GameList>>(json);
			}
			catch
			{
				return new List<GameList>();
			}
		}

		private void SearchResultList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// if (SearchResultList.SelectedItem is string selectedItem)
			// {
			// 	SearchBox.Text = selectedItem;
			// 	// SearchResult.IsOpen = false;
			// }
		}
		private void SearchBoxPopup_OnOpened(object? sender, EventArgs e)
		{
			SearchBox.Focus();
			DimLayer.Visibility = Visibility.Visible;
			NotFoundSearch.Visibility = Visibility.Collapsed;
			SearchResultList.Visibility = Visibility.Collapsed;
			NullSearch.Visibility = Visibility.Visible;
		}
		private async void SearchBoxPopup_OnClosed(object? sender, EventArgs e)
		{
			DimLayer.Visibility = Visibility.Collapsed;
			SearchBox.Text = null;
			_filteredItems.Clear();
		}
		private async void CloseSearch_OnClick(object sender, RoutedEventArgs e)
		{
			await Task.Delay(TimeSpan.FromMilliseconds(1));
			SearchBoxPopup.IsOpen = false;
		}
		private async void AppendSearch_OnClick(object sender, RoutedEventArgs e)
		{
			return;
		}
		#endregion

		private void DownloadLog_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var filePath = Path.Combine(Path.GetTempPath(), "PsxDataHelperLogs.json");
				string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				string destinationPath = Path.Combine(desktopPath, "PsxDataHelperLogs.json");

				if (File.Exists(filePath))
				{
					File.Copy(filePath, destinationPath, true);
				}
				else
				{
				}
			}
			catch (Exception ex)
			{
			}
		}

		private void PKGToolBtn_Click(object sender, RoutedEventArgs e)
		{
			return;
		}

		private void Main_OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && SearchBoxPopup.IsOpen)
			{
				SearchBoxPopup.IsOpen = false;
				e.Handled = true;
			}
		}

		private void AddToDatabase_btn_OnClick(object sender, RoutedEventArgs e)
		{
			AddToDatabase addToDatabase = new AddToDatabase();
			addToDatabase.ShowDialog();
		}

		private void SearchBoxPopup_OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}
	}
}
