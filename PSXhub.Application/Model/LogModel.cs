namespace PSXhub.Application.Model
{
	public class LogModel
	{
		public string? Url { get; set; } = string.Empty;
		public string? Local { get; set; } = string.Empty;
		public bool LocalNotExist { get; set; }
	}

	public class LogsModel
	{
		public List<LogModel> LogModels { get; set; } = new List<LogModel>();
	}
}
