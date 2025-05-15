namespace PSXhub.Application.Model
{
	public class AppSettings
	{
		public int Language { get; set; } = 1;
		public bool ActiveTray { get; set; } = false;
		public bool IsLocalAdmin { get; set; } = false;
		public bool IsAdminNetworkSharingWindow { get; set; } = false;
		public string? SharerAdapter { get; set; } = string.Empty;
		public string? RecipientAdapter { get; set; } = string.Empty;
		public bool HideNetIpAlert { get; set; } = false;
		public string? AutoFinderPath { get; set; } = string.Empty;
		public bool FolderQuestion { get; set; } = false;
		public bool UseOldFolderSelectType { get; set; } = false;
		public bool Database { get; set; } = false;
		public bool AutoFinder { get; set; } = false;
		public bool BypassUpdate { get; set; } = false;
		public List<string> DataBaseExcludePaths { get; set; } = new List<string>();
		public List<string> DataBaseExcludeDrives { get; set; } = new List<string>();
		public bool DownloadByInternetInDatabase { get; set; } = true;
		public bool DownloadByInternetInAutoFinder { get; set; } = true;
		public bool ConfirmScanDatabaseActive { get; set; } = false;
		public bool Debugger { get; set; } = false;
		public bool UpdateAutomatically { get; set; } = true;
		public List<PKGCompareModel> PkgCompareModels { get; set; } = new List<PKGCompareModel>();
	}
}
