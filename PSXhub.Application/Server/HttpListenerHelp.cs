using System.Net;
using System.Net.Sockets;

namespace PSXhub.Application.Server
{
	public sealed class HttpListenerHelp : Listener
	{
		public UpdataUrlLog? UpdataUrlLog;

		public HttpListenerHelp(int port)
			: this(IPAddress.Any, port)
		{
		}

		public HttpListenerHelp(IPAddress address, int port)
			: base(port, address)
		{
		}

		public HttpListenerHelp(IPAddress address, int port, UpdataUrlLog updataurlLog)
			: base(port, address)
		{
			UpdataUrlLog = updataurlLog;
		}

		public override string ConstructString => $"Host:{Address};Port:{Port}";

		public override void OnAccept(IAsyncResult ar)
		{
			try
			{
				Socket? clientSocket = ListenSocket?.EndAccept(ar);
				if (clientSocket != null)
				{
					HttpClient client = new(clientSocket, RemoveClient);
					AddClient(client);
					client.StartHandshake();
				}
			}
			catch
			{
			}
			try
			{
				ListenSocket?.BeginAccept(OnAccept, ListenSocket);
			}
			catch
			{
				Dispose();
			}
		}


		public override string ToString()
		{
			return ($"HTTP service on {Address}:{Port}");
		}
	}
}
