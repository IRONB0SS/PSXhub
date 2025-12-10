using Panuon.WPF.UI;

namespace PSXhub.NetworkSharing.WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : WindowX
	{
		public NetworkSharingViewModel ViewModel;
		public MainWindow()
		{
			InitializeComponent();
			ViewModel = new NetworkSharingViewModel();
			DataContext = ViewModel;
		}

		private void MainWindow_OnClosed(object? sender, EventArgs e)
		{
			System.Windows.Application.Current.Shutdown();
		}
	}
}