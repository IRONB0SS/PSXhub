using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PSXhub.WPF.ViewModel;

namespace PSXhub.WPF
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
	    private string _title;
	    private double _width;
	    private Frame _body;
	    private MainViewModel _viewModel;
		public CustomMessageBox(string title, Frame body, double? width = 0)
        {
            _title = title;
            _body = body;
            _width = width ?? 0;
            InitializeComponent();
            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;
			Setter();
        }

	    private void Setter()
	    {
		    if (_width != 0 && _width != null)
		    {
			    Width = _width;
			    SizeToContent = SizeToContent.Height;
			}
		    else
		    {
			    MinWidth = 360;
			    MinHeight = 220;
			    SizeToContent = SizeToContent.WidthAndHeight;
		    }

		    CTitle.Text = _title;
            Message.Children.Add(_body);
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

		private void CustomMessageBox_OnLoaded(object sender, RoutedEventArgs e)
		{
			this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
			this.Top = (SystemParameters.PrimaryScreenHeight - this.Height) / 3.4;
		}
    }
}
