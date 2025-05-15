using System.Text.Json;
using Newtonsoft.Json;
using PSXhub.Application.Model;

namespace PSXhub.Application.Services
{
	public static class LogService
	{
		private static readonly string FilePath = Path.Combine(Path.GetTempPath(), "PsxDataHelperLogs.json");
		public static event Action<LogModel>? LogAdded;
		public static LogsModel LoadLogs()
		{
			if (!File.Exists(FilePath))
				return new LogsModel();

			string json = File.ReadAllText(FilePath);
			return JsonConvert.DeserializeObject<LogsModel>(json) ?? new LogsModel();
		}

		public static List<LogModel> GetAllLogs()
		{
			var logs = LoadLogs();
			return logs.LogModels;
		}

		public static async Task AddLog(LogModel model)
		{
			try
			{
				List<LogModel> logs = GetAllLogs();

				if (logs.Any(a => a.Url == model.Url && a.Local == model.Local))
				{
					return;
				}

				var log = new LogModel
				{
					Url = model.Url,
					Local = model.Local ?? "Not Found..."
				};

				logs.Add(log);

				LogsModel finalLogs = new LogsModel
				{
					LogModels = logs
				};

				string json = System.Text.Json.JsonSerializer.Serialize(finalLogs, new JsonSerializerOptions { WriteIndented = true });

				using (FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
				using (StreamWriter writer = new StreamWriter(fs))
				{
					await writer.WriteAsync(json);
				}

				// LogAdded?.Invoke("add");
				LogAdded?.Invoke(model);
			}
			catch
			{
			}
		}

		public static void ClearLogs()
		{
			List<LogModel> emptyList = new List<LogModel>();
			LogsModel logs = new LogsModel
			{
				LogModels = emptyList
			};
			string json = System.Text.Json.JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(FilePath, json);
		}
	}
}
