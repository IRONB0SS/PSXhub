using System.Diagnostics;
using Microsoft.Win32;

namespace PSXhub.Application.Services
{
	public static class IdmManagerService
	{
		public static bool IsIDMInstalled(out string idmPath)
		{
			idmPath = null;
			string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Internet Download Manager";
			using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey))
			{
				if (key != null)
				{
					object installLocation = key.GetValue("InstallLocation");
					if (installLocation != null)
					{
						idmPath = Path.Combine(installLocation.ToString(), "IDMan.exe");
						return File.Exists(idmPath);
					}
				}
			}

			string defaultPath = @"C:\Program Files (x86)\Internet Download Manager\IDMan.exe";
			if (File.Exists(defaultPath))
			{
				idmPath = defaultPath;
				return true;
			}

			return false;
		}

		public static void OpenIdmWithLink(string idmPath, string downloadUrl)
		{
			try
			{
				Process.Start(idmPath, $"/d \"{downloadUrl}\"");
			}
			catch (Exception ex)
			{
			}
		}
	}
}
