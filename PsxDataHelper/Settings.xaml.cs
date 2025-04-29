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
using System.Windows.Shapes;
using PsxDataHelper.Application.Model;
using PsxDataHelper.Application.Services;
using PsxDataHelper.UI.ViewModel;

namespace PSXhub
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
	    private MainViewModel _viewModel;
		public Settings()
        {
            InitializeComponent();
			_viewModel = new MainViewModel();
			this.DataContext = _viewModel;
		}

		private void Close_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Header_OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				this.DragMove();
			}
		}

		private void Save_OnClick(object sender, RoutedEventArgs e)
		{
			_viewModel.SaveSettings();
			this.Close();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
