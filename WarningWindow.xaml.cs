using Panuon.WPF.UI;
using PSXhub.NetworkSharing.WPF.ViewModels;

namespace PSXhub.NetworkSharing.WPF
{
	/// <summary>
	/// Interaction logic for WarningWindow.xaml
	/// </summary>
	public partial class WarningWindow : WindowX
	{
		public WarningWindowViewModel ViewModel;
        public WarningWindow()
        {
            InitializeComponent();
			ViewModel = new WarningWindowViewModel();
			DataContext = ViewModel;
        }
    }
}
