using System.Collections;
using System.Net;
using System.Net.Sockets;

namespace PSXhub.Application.Server
{
	public abstract class Listener : IDisposable
	{
		private readonly ArrayList _clients;
		private IPAddress? _mAddress;
		private Socket? _listenSocket;
		private int _port;

		protected Listener(int port, IPAddress address)
		{
			_clients = new ArrayList();
			IsDisposed = false;
			Port = port;
			Address = address;
		}

		public IPAddress? Address
		{
			get => _mAddress;
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(Address));
				}
				_mAddress = value;
				Restart();
			}
		}

		public ArrayList Clients => _clients;

		public abstract string ConstructString { get; }

		public bool IsDisposed { get; private set; }

		public bool Listening => ListenSocket != null;

		public Socket? ListenSocket
		{
			get => _listenSocket;
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(ListenSocket));
				}
				_listenSocket = value;
			}
		}

		public int Port
		{
			get => _port;
			set
			{
				if (value <= 0)
				{
					throw new ArgumentException(nameof(Port));
				}
				_port = value;
				Restart();
			}
		}

		public void Dispose()
		{
			Dispose(IsDisposed);
			if (IsDisposed)
			{
				GC.SuppressFinalize(this);
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				while (Clients.Count > 0)
				{
					((Client)Clients[0]!).Dispose();
				}
				try
				{
					ListenSocket!.Shutdown(SocketShutdown.Both);
				}
				catch
				{
				}
				finally
				{
					ListenSocket?.Close();
					IsDisposed = true;
				}
			}
		}

		public void AddClient(Client client)
		{
			if (Clients.IndexOf(client) == -1)
			{
				Clients.Add(client);
			}
		}

		~Listener()
		{
			Dispose();
		}

		public Client? GetClientAt(int index)
		{
			if ((index >= 0) && (index < GetClientCount()))
			{
				return (Client)Clients[index]!;
			}
			return null;
		}

		public int GetClientCount()
		{
			return Clients.Count;
		}

		public static IPAddress GetLocalExternalIp()
		{
			try
			{
				IPHostEntry entry = Dns.GetHostEntry(Dns.GetHostName());
				foreach (IPAddress t in entry.AddressList)
				{
					if (IsRemoteIp(t))
					{
						return t;
					}
				}
				return entry.AddressList[0];
			}
			catch
			{
				return IPAddress.Any;
			}
		}

		public static IPAddress GetLocalInternalIp()
		{
			try
			{
				IPHostEntry entry = Dns.GetHostEntry(Dns.GetHostName());
				foreach (IPAddress t in entry.AddressList)
				{
					if (IsLocalIp(t))
					{
						return t;
					}
				}
				return entry.AddressList[0];
			}
			catch
			{
				return IPAddress.Any;
			}
		}

		public static bool IsLocalIp(IPAddress ip)
		{
			byte num = (byte)Math.Floor((double)(ip.GetAddressBytes().LongLength % 256));
			byte num2 = (byte)Math.Floor((double)(ip.GetAddressBytes().LongLength % 65536 / 256));
			return (num == 10) || ((num == 0xac) && (num2 >= 0x10) && (num2 <= 0x1f)) ||
					((num == 0xc0) && (num2 == 0xa8));
		}

		public static bool IsRemoteIp(IPAddress ip)
		{
			byte num = (byte)Math.Floor((double)(ip.GetAddressBytes().LongLength % 256));
			byte num2 = (byte)Math.Floor((double)(ip.GetAddressBytes().LongLength % 65536 / 256));
			return (num != 10) && ((num != 0xac) || (num2 < 0x10) || (num2 > 0x1f)) &&
					  ((num != 0xc0) || (num2 != 0xa8)) &&
					 (!ip.Equals(IPAddress.Any) && !ip.Equals(IPAddress.Loopback)) && !ip.Equals(IPAddress.Broadcast);
		}

		public abstract void OnAccept(IAsyncResult ar);

		public void RemoveClient(Client client)
		{
			try
			{
				if (client != null && Clients.Contains(client))
				{
					Clients.Remove(client);
				}
			}
			catch { }
		}

		public void Restart()
		{
			if (ListenSocket != null)
			{
				ListenSocket.Close();
				Start();
			}
		}

		public void Start()
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
			try
			{
				ListenSocket = new Socket(Address!.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				ListenSocket.Bind(new IPEndPoint(Address, Port));
				ListenSocket.Listen(50);
				ListenSocket.ReceiveBufferSize = 8192;
				ListenSocket.BeginAccept(OnAccept, ListenSocket);
			}
			catch
			{
				ListenSocket.Close(); // this
				throw new SocketException();
			}
		}

		public abstract override string ToString();
	}
}
