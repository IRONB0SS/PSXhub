using System.Windows;
using System.Windows.Controls;

namespace PSXhub.WPF
{
    /// <summary>
    /// Interaction logic for DonateBitcoinPage.xaml
    /// </summary>
    public partial class DonateBitcoinPage : Page
    {
        public DonateBitcoinPage()
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
    }
}
