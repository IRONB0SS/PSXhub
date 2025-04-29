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
    /// Interaction logic for DatabaseGuidePage.xaml
    /// </summary>
    public partial class DatabaseGuidePage : Page
    {
	    private MainViewModel _viewModel;
		public DatabaseGuidePage()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;
		}
    }
}
