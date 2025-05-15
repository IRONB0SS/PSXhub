using System.Windows;
using System.Windows.Controls;

namespace PSXhub.WPF
{
    /// <summary>
    /// Interaction logic for DonatePage.xaml
    /// </summary>
    public partial class DonatePage : Page
    {
        public DonatePage()
        {
            InitializeComponent();
        }

		private async void BTC_Copy_OnClick(object sender, RoutedEventArgs e)
		{
			Button btn = sender as Button;
			if (btn?.Tag != null)
			{
				Clipboard.SetText(btn.Tag.ToString());
			}

			BTC_Copy_Text.Text = "Copied!";
			BTC_Copy.IsEnabled = false;

			await Task.Delay(2000);

			BTC_Copy_Text.Text = "Copy Address";
			BTC_Copy.IsEnabled = true;
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
