using System.Windows.Controls;
using PSXhub.WPF.ViewModel;

namespace PSXhub.WPF
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
