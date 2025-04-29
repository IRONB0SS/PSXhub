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
using PsxDataHelper.UI.ViewModel;

namespace PSXhub
{
    /// <summary>
    /// Interaction logic for SelectFilesInfoPage.xaml
    /// </summary>
    public partial class SelectFilesInfoPage : Page
    {
	    private MainViewModel _viewModel;
		public SelectFilesInfoPage()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;
		}
    }
}
