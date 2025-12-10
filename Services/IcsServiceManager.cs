using System.Management;
using System.ServiceProcess;

namespace PSXhub.NetworkSharing.WPF.Services
{
	public static class IcsServiceManager
	{
		public static void RestartICSService()
		{
			try
			{
				ServiceController service = new ServiceController("SharedAccess");

				if (service.Status == ServiceControllerStatus.Running)
				{
					service.Stop();
					service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
				}

				service.Start();
				service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
			}
			catch (Exception ex)
			{
				return;
			}
		}


		public static void SetAdapterToDhcpByGuid(string adapterGuid)
		{
			ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
			ManagementObjectCollection moc = mc.GetInstances();

			foreach (ManagementObject mo in moc)
			{
				try
				{
					if (mo["IPEnabled"] is bool ipEnabled && ipEnabled)
					{
						string settingId = mo["SettingID"]?.ToString();
						if (!string.IsNullOrEmpty(settingId) &&
						    string.Equals(settingId, adapterGuid, StringComparison.OrdinalIgnoreCase))
						{
							// فعال‌سازی DHCP
							mo.InvokeMethod("EnableDHCP", null);
							mo.InvokeMethod("SetDNSServerSearchOrder", null);

							Console.WriteLine($"Adapter with GUID '{adapterGuid}' set to DHCP and automatic DNS.");
							return; // کار انجام شد، نیازی به ادامه نیست
						}
					}
				}
				catch (ManagementException mex)
				{
					Console.WriteLine($"WMI error on adapter '{adapterGuid}': {mex.Message}");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Unexpected error on adapter '{adapterGuid}': {ex.Message}");
				}
			}

			Console.WriteLine($"No adapter found with GUID '{adapterGuid}'.");
		}

		public static void StopSharing()
		{
			try
			{
				ManagementScope scope = new ManagementScope(@"\\.\root\Microsoft\HomeNet");
				scope.Connect();

				ManagementObjectSearcher searcher =
					new ManagementObjectSearcher(scope, new ObjectQuery("SELECT * FROM HNet_Connection"));
				foreach (ManagementObject queryObj in searcher.Get())
				{
					try
					{
						queryObj.Delete();
					}
					catch (Exception ex)
					{
					}
				}
			}
			catch (Exception ex)
			{
			}

			RestartICSService();
		}
	}
}