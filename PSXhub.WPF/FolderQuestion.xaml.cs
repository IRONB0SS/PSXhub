using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PSXhub.Application.Model;
using PSXhub.Application.Services;
using PSXhub.WPF.ViewModel;

namespace PSXhub.WPF
{
    /// <summary>
    /// Interaction logic for FolderQuestion.xaml
    /// </summary>
    public partial class FolderQuestion : Window
    {
	    public AppSettings Settings => SettingsManager.Instance.AppSettings;
	    private MainViewModel _viewModel;
		public FolderQuestion()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;
		}

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
	        var selectedRadio = FindLogicalChildren<RadioButton>(this)
		        .FirstOrDefault(r => r.GroupName == "browser" && r.IsChecked == true);

	        if (selectedRadio != null)
	        {
		        Settings.FolderQuestion = true;
		        Settings.UseOldFolderSelectType = Convert.ToBoolean(selectedRadio.Tag?.ToString());
		        SettingsManager.Instance.SaveSettings();
				this.Close();
	        }
	        else
	        {
		        MessageBox.Show("هیچ گزینه‌ای انتخاب نشده است!");
	        }
		}

        public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject parent) where T : DependencyObject
        {
	        if (parent == null) yield break;

	        foreach (object child in LogicalTreeHelper.GetChildren(parent))
	        {
		        if (child is T tChild)
			        yield return tChild;

		        if (child is DependencyObject dependencyChild)
		        {
			        foreach (var descendant in FindLogicalChildren<T>(dependencyChild))
			        {
				        yield return descendant;
			        }
		        }
	        }
        }

        private void OldBrowserBorder_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
	        OldBrowserRadioButton.IsChecked = true;
        }

        private void ModernBrowserBorder_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
	        ModernBrowserRadioButton.IsChecked = true;
        }
    }
}
