using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PSXhub
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
