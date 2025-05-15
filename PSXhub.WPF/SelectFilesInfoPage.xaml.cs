using System.Windows.Controls;
using PSXhub.WPF.ViewModel;

namespace PSXhub.WPF
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
