using System.Net;
using System.Net.Sockets;

namespace PSXhub.Application.Server
{
	public delegate void DestroyDelegate(Client client);
	public delegate void UpdataUrlLog(UrlInfo urlinfo);

	public class PSXTools
	{
		public static List<IPAddress> GetHostIp()
		{
			IPHostEntry ip = Dns.GetHostEntry(Dns.GetHostName());
			List<IPAddress> iplist = ip.AddressList.Where(p => p.AddressFamily == AddressFamily.InterNetwork).ToList();
			return iplist;
		}

		// public static bool RegexUrl(string? urls)
		// {
		// 	string[]? rules = AppConfig.Instance().Rule!.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
		// 	if (rules.Length <= 0 || string.IsNullOrEmpty(urls))
		// 	{
		// 		return false;
		// 	}
		//
		// 	return
		// 		rules.Select(rule => new Regex(rule.ToLower().Replace(".", @"\.").Replace("*", ".*?")))
		// 			.Any(regex => regex.Match(urls.ToLower()).Success);
		// }
	}
}
