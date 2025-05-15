namespace PSXhub.Application.Model
{
	public class PKGCompareModel
	{
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public string? Hash { get; set; } = null;
		public bool? IsMatch { get; set; } = null;
	}
}
