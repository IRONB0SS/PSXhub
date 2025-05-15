using System.Windows;
using System.Windows.Input;
using PSXhub.WPF.ViewModel;

namespace PSXhub.WPF
{
	/// <summary>
	/// Interaction logic for AddToDatabase.xaml
	/// </summary>
	public partial class AddToDatabase : Window
	{
		private MainViewModel _viewModel;
		public AddToDatabase()
		{
			_viewModel = new MainViewModel();
			this.DataContext = _viewModel;
			InitializeComponent();
		}
		private void Ok_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Close_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void TitleBar_OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				this.DragMove();
			}
		}

		private async void SendPS4_OnClick(object sender, RoutedEventArgs e)
		{
			return;
		}

		private async void SendPS5_OnClick(object sender, RoutedEventArgs e)
		{
			return;
		}

		private void PS5TextBox_OnKeyUp(object sender, KeyEventArgs e)
		{
			if (string.IsNullOrEmpty(PS5TextBox.Text))
			{
				SendPS5.IsEnabled = false;
			}
			else
			{
				SendPS5.IsEnabled = true;
			}
		}

		private void PS4TextBox_OnKeyUp(object sender, KeyEventArgs e)
		{
			if (string.IsNullOrEmpty(PS4TextBox.Text))
			{
				SendPS4.IsEnabled = false;
			}
			else
			{
				SendPS4.IsEnabled = true;
			}
		}
	}
}
