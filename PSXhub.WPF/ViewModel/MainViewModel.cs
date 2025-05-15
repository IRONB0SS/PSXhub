using System.Collections.ObjectModel;
using System.Globalization;
using System.Resources;
using System.Windows.Input;
using PSXhub.Application.Model;
using PSXhub.Application.Services;

namespace PSXhub.WPF.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		public event Action? MessageChanged;
		public ObservableCollection<string> Logs { get; } = new ObservableCollection<string>();
		private ObservableCollection<LogModel> Log { get; set; }
		public ObservableCollection<Language> Languages { get; set; }
		public AppSettings Settings => SettingsManager.Instance.AppSettings;
		private ResourceManager _resourceManager = new ResourceManager(typeof(Resources.Resources));
		public string this[string key] =>
			_resourceManager.GetString(key)
				.Replace("\\n", "\r")
			?? key;


		private bool _isAutoFinderChecked;
		private bool _isDatabaseChecked;
		private bool _isBypassUpdateChecked;
		private bool _confirmScanDatabaseActive;
		private string _font;
		private string _font2;
		private bool _downloadByInternetInDatabase;
		private bool _downloadByInternetInAutoFinder;
		private bool _debugger;
		private bool _activeTray;
		private bool _updateAutomatically;
		private bool _showUpdateBtn;
		private Language _language;

		public bool IsAutoFinderChecked
		{
			get { return _isAutoFinderChecked; }
			set
			{
				if (_isAutoFinderChecked == true)
				{
					_isAutoFinderChecked = false;
					OnPropertyChanged(nameof(IsAutoFinderChecked));
					Settings.AutoFinder = false;
					SettingsManager.Instance.SaveSettings();
				}
				else if (_isAutoFinderChecked == false)
				{
					_isAutoFinderChecked = true;
					OnPropertyChanged(nameof(IsAutoFinderChecked));
					Settings.AutoFinder = true;
					SettingsManager.Instance.SaveSettings();
				}
			}
		}
		public bool IsDatabaseChecked
		{
			get { return _isDatabaseChecked; }
			set
			{
				if (_isDatabaseChecked == true)
				{
					_isDatabaseChecked = false;
					OnPropertyChanged(nameof(IsDatabaseChecked));
					Settings.Database = false;
					SettingsManager.Instance.SaveSettings();
				}
				else if (_isDatabaseChecked == false)
				{
					_isDatabaseChecked = true;
					OnPropertyChanged(nameof(IsDatabaseChecked));
					Settings.Database = true;
					SettingsManager.Instance.SaveSettings();
				}
			}
		}

		public bool IsBypassUpdateChecked
		{
			get
			{
				return _isBypassUpdateChecked;
			}
			set
			{
				_isBypassUpdateChecked = value;
				Settings.BypassUpdate = value;
				SettingsManager.Instance.SaveSettings();
			}
		}

		public bool DownloadByInternetInDatabase
		{
			get
			{
				return _downloadByInternetInDatabase;
			}
			set
			{
				_downloadByInternetInDatabase = value;
			}
		}
		public bool DownloadByInternetInAutoFinder
		{
			get
			{
				return _downloadByInternetInAutoFinder;
			}
			set
			{
				_downloadByInternetInAutoFinder = value;
			}
		}
		public bool Debugger
		{
			get
			{
				return _debugger;
			}
			set
			{
				_debugger = value;
			}
		}
		public bool ConfirmScanDatabaseActive
		{
			get { return _confirmScanDatabaseActive; }
			set
			{
				_confirmScanDatabaseActive = value;
			}
		}
		public string Font
		{
			get
			{
				return _font;
			}
			set
			{
				_font = value;
			}
		}

		public string Font2
		{
			get
			{
				return _font2;
			}
			set
			{
				_font2 = value;
			}
		}

		public bool ActiveTray
		{
			get
			{
				return _activeTray;
			}
			set
			{
				_activeTray = value;
			}
		}

		public bool UpdateAutomatically
		{
			get
			{
				return _updateAutomatically;
			}
			set
			{
				_updateAutomatically = value;

			}
		}

		public bool ShowUpdateBtn
		{
			get
			{
				return _showUpdateBtn;
			}
			set
			{
				_showUpdateBtn = value;

			}
		}

		public Language Language
		{
			get => _language;
			set
			{
				_language = value;
				OnPropertyChanged(nameof(Language));
			}
		}

		public ICommand ChangeMessageCommand { get; }

		public MainViewModel()
		{
			_isAutoFinderChecked = Settings.AutoFinder;
			_isDatabaseChecked = Settings.Database;
			_isBypassUpdateChecked = Settings.BypassUpdate;
			_confirmScanDatabaseActive = Settings.ConfirmScanDatabaseActive;
			_downloadByInternetInDatabase = Settings.DownloadByInternetInDatabase;
			_downloadByInternetInAutoFinder = Settings.DownloadByInternetInAutoFinder;
			_debugger = Settings.Debugger;
			_updateAutomatically = Settings.UpdateAutomatically;
			_showUpdateBtn = !Settings.UpdateAutomatically;
			_font = PSXhub.Properties.Settings.Default.BlinkMacSystemFont;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(Language.GetById(Settings.Language).Symbol);
			_font2 = Thread.CurrentThread.CurrentUICulture.Name == "fa"
				? PSXhub.Properties.Settings.Default.Vazir
				: PSXhub.Properties.Settings.Default.BlinkMacSystemFont;
			_activeTray = Settings.ActiveTray;
			Languages = new ObservableCollection<Language>(Language.DefaultLanguages);
			_language = Language.GetById(Settings.Language);
			Settings.PkgCompareModels.Clear();
			SettingsManager.Instance.SaveSettings();
		}

		public void SaveSettings()
		{
			Settings.DownloadByInternetInDatabase = _downloadByInternetInDatabase;
			Settings.DownloadByInternetInAutoFinder = _downloadByInternetInAutoFinder;
			Settings.ConfirmScanDatabaseActive = _confirmScanDatabaseActive;
			Settings.Debugger = _debugger;
			Settings.ActiveTray = _activeTray;
			Settings.UpdateAutomatically = _updateAutomatically;
			Settings.Language = _language.Id;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(_language.Symbol);
			SettingsManager.Instance.SaveSettings();
		}
	}
}
