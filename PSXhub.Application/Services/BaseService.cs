using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text.RegularExpressions;

namespace PSXhub.Application.Services
{
	public static class BaseService
	{
		public static string ShortenText(string? text, int maxLength)
		{
			if (string.IsNullOrEmpty(text))
				return null;

			if (text.Length <= maxLength)
				return text;

			int firstPartLength = (maxLength - 3) / 2;
			int lastPartLength = maxLength - firstPartLength - 3;

			return text.Substring(0, firstPartLength) + "..." + text.Substring(text.Length - lastPartLength);
		}

		public static Array GetIPs()
		{
			try
			{
				var ips = Dns.GetHostEntry(Dns.GetHostName());
				return ips.AddressList.Where(p => p.AddressFamily == AddressFamily.InterNetwork).ToArray();
			}
			catch
			{
				return null;
			}
		}

		public static Array GetNetworkInterfaces()
		{
			try
			{
				NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				return networkInterfaces;
			}
			catch
			{
				return null;
			}
		}

		public static void OpenNetworkConnections()
		{
			try
			{
				ProcessStartInfo psi = new ProcessStartInfo
				{
					FileName = "rundll32.exe",
					Arguments = "shell32.dll,Control_RunDLL ncpa.cpl",
					UseShellExecute = true,
					// Verb = "runas"
				};

				Process.Start(psi);
			}
			catch
			{
				throw;
			}
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
			}
		}

		public static void EnableDHCP(string interfaceName)
		{
			try
			{
				ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
				ManagementObjectCollection moc = mc.GetInstances();

				foreach (ManagementObject mo in moc)
				{
					if ((bool)mo["IPEnabled"] && mo["Description"].ToString() == interfaceName)
					{
						ManagementBaseObject enableDhcp = mo.InvokeMethod("EnableDHCP", null, null);

						ManagementBaseObject setDns = mo.InvokeMethod("SetDNSServerSearchOrder", null, null);
						break;
					}
				}
			}
			catch (Exception ex)
			{
			}
		}

		public static void EnableDHCPForAllInterfaces()
		{
			try
			{
				ManagementObjectSearcher searcher =
					new ManagementObjectSearcher(
						"SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionID IS NOT NULL");
				foreach (ManagementObject adapter in searcher.Get())
				{
					string interfaceName = adapter["NetConnectionID"].ToString();
					Process process = new Process
					{
						StartInfo = new ProcessStartInfo
						{
							FileName = "powershell",
							Arguments = $"netsh interface ip set address name=\"{interfaceName}\" source=dhcp",
							RedirectStandardOutput = true,
							UseShellExecute = false,
							CreateNoWindow = true
						}
					};
					process.Start();
					string result = process.StandardOutput.ReadToEnd();
					process.WaitForExit();
				}
			}
			catch (Exception ex)
			{
			}
		}

		public static bool CheckICSPresent()
		{
			try
			{
				ManagementObjectSearcher searcher =
					new ManagementObjectSearcher(
						"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = TRUE");

				foreach (ManagementObject obj in searcher.Get())
				{
					if (obj["IPEnabled"] != null && (bool)obj["IPEnabled"] &&
					    obj["InternetConnectionSharing"] != null && (bool)obj["InternetConnectionSharing"])
					{
						return true;
					}
				}
			}
			catch (Exception ex)
			{
			}

			return false;
		}

		public static void Loger(Exception ex)
		{
			string filePath = Path.Combine(Path.GetTempPath(), "PsxDataHelperErrors.txt");
			try
			{
				File.AppendAllText(filePath, ex.Message + Environment.NewLine);
			}
			catch
			{
			}
		}

		public static string LinkTo(string url)
		{
			Regex reg1 = new Regex(@"(PPSA\d{5})");
			Regex reg2 = new Regex(@"(CUSA\d{5})");

			Match match1 = reg1.Match(url);
			Match match2 = reg2.Match(url);

			if (match1.Success)
			{
				return "https://prosperopatches.com/" + match1.Value;
			}

			if (match2.Success)
			{
				return "http://Orbispatches.com/" + match2.Value;
			}

			return null;
		}
	}
}
