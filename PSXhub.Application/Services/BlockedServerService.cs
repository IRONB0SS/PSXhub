using System.ComponentModel;
using System.Text.Json;

namespace PSXhub.Application.Services
{
	public class BlockedServerService
	{
		private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BlockedServer.json");
		private static BlockedServerService _instance;

		public static BlockedServerService Instance => _instance ??= LoadServers();

		public List<string> BlockedServers { get; set; } = new List<string>();

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private static BlockedServerService LoadServers()
		{
			if (File.Exists(FilePath))
			{
				try
				{
					string json = File.ReadAllText(FilePath);
					var test = JsonSerializer.Deserialize<BlockedServerService>(json) ?? new BlockedServerService();
					return test;
				}
				catch
				{
					return new BlockedServerService();
				}
			}
			return new BlockedServerService();
		}
	}
}
