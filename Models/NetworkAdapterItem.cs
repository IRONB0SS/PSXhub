using NETCONLib;
using System.Net.NetworkInformation;
using ICS;

namespace PSXhub.NetworkSharing.WPF.Models
{
	public class NetworkAdapterItem
	{
		public string DisplayName { get; set; }
		public INetConnection Interface { get; set; }

		public NetworkAdapterItem(string displayName, INetConnection adapter = null)
		{
			DisplayName = displayName;
			Interface = adapter;
		}

		public override string ToString() => DisplayName;
	}

}
