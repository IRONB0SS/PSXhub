using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PSXhub.Application.Model;

namespace PSXhub.WPF
{
    /// <summary>
    /// Interaction logic for ComparePath.xaml
    /// </summary>
    public partial class ComparePath : UserControl
    {
	    private PKGCompareModel _model;
        public ComparePath(PKGCompareModel model)
        {
            _model = model;
            InitializeComponent();
			SetCases();
        }

        private void SetCases()
        {
	        var matchIconPath = Geometry.Parse("M1.5 3.25a2.25 2.25 0 1 1 3 2.122v5.256a2.251 2.251 0 1 1-1.5 0V5.372A2.25 2.25 0 0 1 1.5 3.25Zm5.677-.177L9.573.677A.25.25 0 0 1 10 .854V2.5h1A2.5 2.5 0 0 1 13.5 5v5.628a2.251 2.251 0 1 1-1.5 0V5a1 1 0 0 0-1-1h-1v1.646a.25.25 0 0 1-.427.177L7.177 3.427a.25.25 0 0 1 0-.354ZM3.75 2.5a.75.75 0 1 0 0 1.5.75.75 0 0 0 0-1.5Zm0 9.5a.75.75 0 1 0 0 1.5.75.75 0 0 0 0-1.5Zm8.25.75a.75.75 0 1 0 1.5 0 .75.75 0 0 0-1.5 0Z");
	        var notMatchIconPath = Geometry.Parse("M3.25 1A2.25 2.25 0 0 1 4 5.372v5.256a2.251 2.251 0 1 1-1.5 0V5.372A2.251 2.251 0 0 1 3.25 1Zm9.5 5.5a.75.75 0 0 1 .75.75v3.378a2.251 2.251 0 1 1-1.5 0V7.25a.75.75 0 0 1 .75-.75Zm-2.03-5.273a.75.75 0 0 1 1.06 0l.97.97.97-.97a.748.748 0 0 1 1.265.332.75.75 0 0 1-.205.729l-.97.97.97.97a.751.751 0 0 1-.018 1.042.751.751 0 0 1-1.042.018l-.97-.97-.97.97a.749.749 0 0 1-1.275-.326.749.749 0 0 1 .215-.734l.97-.97-.97-.97a.75.75 0 0 1 0-1.06ZM2.5 3.25a.75.75 0 1 0 1.5 0 .75.75 0 0 0-1.5 0ZM3.25 12a.75.75 0 1 0 0 1.5.75.75 0 0 0 0-1.5Zm9.5 0a.75.75 0 1 0 0 1.5.75.75 0 0 0 0-1.5Z");
	        var nullMatchIconPath = Geometry.Parse("M9.573.677A.25.25 0 0 1 10 .854V2.5h1A2.5 2.5 0 0 1 13.5 5v5.628a2.251 2.251 0 1 1-1.5 0V5a1 1 0 0 0-1-1h-1v1.646a.25.25 0 0 1-.427.177L7.177 3.427a.25.25 0 0 1 0-.354ZM6 12v-1.646a.25.25 0 0 1 .427-.177l2.396 2.396a.25.25 0 0 1 0 .354l-2.396 2.396A.25.25 0 0 1 6 15.146V13.5H5A2.5 2.5 0 0 1 2.5 11V5.372a2.25 2.25 0 1 1 1.5 0V11a1 1 0 0 0 1 1ZM4 3.25a.75.75 0 1 0-1.5 0 .75.75 0 0 0 1.5 0ZM12.75 12a.75.75 0 1 0 0 1.5.75.75 0 0 0 0-1.5Z");
	        var matchIconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3FB950"));
	        var notMatchIconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F85149"));
	        var nullMatchIconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9198A1"));

	        Path.Text = _model.FileName;
	        PathToggleButton.Tag = _model;
	        if (_model.IsMatch == null)
	        {
		        Icon.Data = nullMatchIconPath;
				Icon.Fill = nullMatchIconColor;
	        }
	        else if (_model.IsMatch == true)
	        {
		        Icon.Data = matchIconPath;
		        Icon.Fill = matchIconColor;
	        }
	        else
	        {
		        Icon.Data = notMatchIconPath;
				Icon.Fill = notMatchIconColor;
				PathCheckBox.IsChecked = true;
				PathToggleButton.IsChecked = true;
	        }
        }

        private void PathToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
	        PathCheckBox.IsChecked = PathToggleButton.IsChecked;
        }

        private void PathCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
	        PathToggleButton.IsChecked = PathCheckBox.IsChecked;
        }

        private void PathToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
			PathCheckBox.IsChecked = PathToggleButton.IsChecked;
		}

        private void PathCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
			PathToggleButton.IsChecked = PathCheckBox.IsChecked;
		}
    }
}
