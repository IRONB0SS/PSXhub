using System.ComponentModel;
using System.Text.Json;
using PSXhub.Application.Model;

namespace PSXhub.Application.Services
{
	public class SettingsManager : INotifyPropertyChanged
	{
		private static readonly string FilePath = Path.Combine(Path.GetTempPath(), "PsxDataHelperSettings.json");
		private static SettingsManager _instance;

		public static SettingsManager Instance => _instance ??= LoadSettings();

		public AppSettings AppSettings { get; set; } = new AppSettings();

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private static SettingsManager LoadSettings()
		{
			if (File.Exists(FilePath))
			{
				try
				{
					string json = File.ReadAllText(FilePath);
					return JsonSerializer.Deserialize<SettingsManager>(json) ?? new SettingsManager();
				}
				catch
				{
					return new SettingsManager();
				}
			}
			return new SettingsManager();
		}
		public void SaveSettings()
		{
			string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(FilePath, json);
		}
	}
}
