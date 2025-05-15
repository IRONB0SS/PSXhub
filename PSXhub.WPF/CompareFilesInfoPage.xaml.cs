using System.Windows.Controls;
using PSXhub.WPF.ViewModel;

namespace PSXhub.WPF
{
    /// <summary>
    /// Interaction logic for CompareFilesInfoPage.xaml
    /// </summary>
    public partial class CompareFilesInfoPage : Page
    {
	    private MainViewModel _viewModel;
		public CompareFilesInfoPage()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;
		}
    }
}
