using System.Windows;
using System.Windows.Controls;

namespace PSXhub.WPF
{
	/// <summary>
	/// Interaction logic for DonateTetherPage.xaml
	/// </summary>
	public partial class DonateTetherPage : Page
	{
		public DonateTetherPage()
		{
			InitializeComponent();
		}

		private async void USDT_Copy_OnClick(object sender, RoutedEventArgs e)
		{
			Button btn = sender as Button;
			if (btn?.Tag != null)
			{
				Clipboard.SetText(btn.Tag.ToString());
			}

			USDT_Copy_Text.Text = "Copied!";
			USDT_Copy.IsEnabled = false;

			await Task.Delay(2000);

			USDT_Copy_Text.Text = "Copy Address";
			USDT_Copy.IsEnabled = true;
		}
	}
}
