using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using PSXhub.Application.Model;
using PSXhub.Application.Services;

namespace PSXhub.Application.Server
{
	public sealed class HttpClient : Client
	{
		private string? _mHttpPost;
		private LocalFile? _mLocalFile;
		private readonly UrlInfo _uinfo = new();
		public AppSettings Settings => SettingsManager.Instance.AppSettings;
		public List<string> _blockedServers => BlockedServerService.Instance.BlockedServers;

		public HttpClient(Socket clientSocket, DestroyDelegate destroyer) : base(clientSocket, destroyer)
		{
		}

		private Dictionary<string, string>? HeaderFields { get; set; }

		private string? HttpQuery { get; set; }

		private string? HttpRequestType { get; set; }

		private string? HttpVersion { get; set; }

		public string? RequestedPath { get; set; }

		public string? RequestedUrl { get; set; }

		private async Task QueryHandle(string query)
		{
			HeaderFields = ParseQuery(query);
			if ((HeaderFields == null) || !HeaderFields.ContainsKey("Host"))
			{
				SendBadRequest();
			}
			else
			{
				int num;
				string requestedPath;
				int index;
				string str;
				if (HttpRequestType!.ToUpper().Equals("CONNECT"))
				{
					index = RequestedPath!.IndexOf(":");
					if (index >= 0)
					{
						requestedPath = RequestedPath[..index];
						num = RequestedPath.Length > (index + 1) ? int.Parse(RequestedPath[(index + 1)..]) : 443;
						// await LogService.AddLog(new LogModel
						// {
						// 	Url = requestedPath + " + " + query,
						// 	Local = "Net Connect Request if"
						// });
					}
					else
					{
						requestedPath = RequestedPath;
						num = 80;
						// await LogService.AddLog(new LogModel
						// {
						// 	Url = requestedPath + " + " + query,
						// 	Local = "Net Connect Request else"
						// });
					}
				}
				else
				{
					index = HeaderFields["Host"].IndexOf(":");
					if (index > 0)
					{
						requestedPath = HeaderFields["Host"][..index];
						num = int.Parse(HeaderFields["Host"][(index + 1)..]);
						// await LogService.AddLog(new LogModel
						// {
						// 	Url = requestedPath + " + " + query,
						// 	Local = "Net Host Request"
						// });
					}
					else
					{
						requestedPath = HeaderFields["Host"];
						num = 80;
						// await LogService.AddLog(new LogModel
						// {
						// 	Url = requestedPath + " + " + query,
						// 	Local = "Net Host Request"
						// });
					}

					if (HttpRequestType.ToUpper().Equals("POST"))
					{
						int tempnum = query.IndexOf("\r\n\r\n");
						_mHttpPost = query[(tempnum + 4)..];
						// await LogService.AddLog(new LogModel
						// {
						// 	Url = requestedPath + " + " + query,
						// 	Local = "Net Host Request"
						// });
					}
				}

				string url = FileService.QueryToUrl(query!);
				if (Settings.BypassUpdate && _blockedServers.Any(a => url.Contains(a)))
				{
					// await NetHandle(num, requestedPath);
					Blocking();
					await LogService.AddLog(new LogModel
					{
						Url = url,
						Local = "Blocked..."
					});
				}
				else
				{
					if (!HttpRequestType.ToUpper().Equals("CONNECT"))
					{

						if (FileService.CheckIsFile(url))
						{
							string fileName = FileService.UrlToFileName(url);

							if (Settings.AutoFinder)
							{
								if (!string.IsNullOrEmpty(Settings.AutoFinderPath))
								{
									string autoFinderFile = Path.Combine(Settings.AutoFinderPath!, fileName);

									if (File.Exists(autoFinderFile))
									{
										await LogService.AddLog(new LogModel
										{
											Url = url,
											Local = autoFinderFile,
											LocalNotExist = false
										});
										SendLocalFile(autoFinderFile,
											HeaderFields.ContainsKey("Range") ? HeaderFields["Range"] : null,
											HeaderFields.ContainsKey("Proxy-Connection")
												? HeaderFields["Proxy-Connection"]
												: null);
									}

									else if (!File.Exists(autoFinderFile) && Settings.DownloadByInternetInAutoFinder)
									{
										await NetHandle(num, requestedPath);
										await LogService.AddLog(new LogModel
										{
											Url = url,
											Local =
												"PKG file not found... You're downloading the PKG from Sony servers.",
											LocalNotExist = true
										});
									}

									else
									{
										await LogService.AddLog(new LogModel
										{
											Url = url,
											Local =
												"PKG file not found... You have blocked downloads from Sony servers.",
											LocalNotExist = true
										});
									}
								}
							}
							else if (!Settings.AutoFinder && !Settings.Database)
							{
								await NetHandle(num, requestedPath);
								await LogService.AddLog(new LogModel
								{
									Url = url,
									Local = "You're downloading the PKG from Sony servers.",
									LocalNotExist = true
								});
							}
						}

						else
						{
							try
							{
								IPAddress hostIp = Dns.GetHostEntry(requestedPath).AddressList[0];
								IPEndPoint remoteEp = new(hostIp, num);
								DestinationSocket = new Socket(remoteEp.AddressFamily, SocketType.Stream,
									ProtocolType.Tcp);
								if (HeaderFields.ContainsKey("Proxy-Connection") &&
									HeaderFields["Proxy-Connection"].ToLower().Equals("keep-alive"))
								{
									DestinationSocket.SetSocketOption(SocketOptionLevel.Socket,
										SocketOptionName.KeepAlive, 1);
								}

								DestinationSocket.BeginConnect(remoteEp, OnConnected, DestinationSocket);

								// await LogService.AddLog(new LogModel
								// {
								// 	Url = requestedPath + " + " + query,
								// 	Local = "Net Get Request"
								// });
							}
							catch (Exception ex)
							{
								BaseService.Loger(ex);
								SendBadRequest();
							}
						}
					}
					else
					{
						try
						{
							IPAddress hostIp = Dns.GetHostEntry(requestedPath).AddressList[0];
							IPEndPoint remoteEp = new(hostIp, num);
							DestinationSocket = new Socket(remoteEp.AddressFamily, SocketType.Stream,
								ProtocolType.Tcp);
							if (HeaderFields.ContainsKey("Proxy-Connection") &&
								HeaderFields["Proxy-Connection"].ToLower().Equals("keep-alive"))
							{
								DestinationSocket.SetSocketOption(SocketOptionLevel.Socket,
									SocketOptionName.KeepAlive, 1);
							}

							DestinationSocket.BeginConnect(remoteEp, OnConnected, DestinationSocket);
						}
						catch (Exception ex)
						{
							BaseService.Loger(ex);
							SendBadRequest();
						}
					}
				}
			}
		}

		private async Task NetHandle(int num, string requestedPath)
		{
			try
			{
				IPAddress hostIp = Dns.GetHostEntry(requestedPath).AddressList[0];
				IPEndPoint remoteEp = new(hostIp, num);
				DestinationSocket = new Socket(remoteEp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				if (HeaderFields.ContainsKey("Proxy-Connection") &&
					HeaderFields["Proxy-Connection"].ToLower().Equals("keep-alive"))
				{
					DestinationSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);
				}

				DestinationSocket.BeginConnect(remoteEp, OnConnected, DestinationSocket);

				// await LogService.AddLog(new LogModel
				// {
				// 	Url = requestedPath,
				// 	Local = "Net Handle Request"
				// });
			}
			catch
			{
			}
		}

		private Dictionary<string, string> ParseQuery(string query)
		{
			int index;
			Dictionary<string, string> dictionary = new();
			string[] strArray = query.Replace("\r\n", "\n").Split(new[] { '\n' });
			if (strArray.Length > 0)
			{
				index = strArray[0].IndexOf(' ');
				if (index > 0)
				{
					HttpRequestType = strArray[0][..index];
					strArray[0] = strArray[0][index..].Trim();
				}
				index = strArray[0].LastIndexOf(' ');
				if (index > 0)
				{
					HttpVersion = strArray[0][index..].Trim();
					RequestedUrl = strArray[0][..index];
				}
				else
				{
					RequestedUrl = strArray[0];
				}
				if (!string.IsNullOrEmpty(RequestedUrl) && RequestedUrl.ToLower().StartsWith("http"))
				{
					if (RequestedUrl.ToLower().StartsWith("http://"))
					{
						index = RequestedUrl.IndexOf('/', 7);
					}
					else if (RequestedUrl.ToLower().StartsWith("https://"))
					{
						index = RequestedUrl.IndexOf('/', 8);
					}

					RequestedPath = index == -1 ? "/" : RequestedUrl[index..];
				}
				else
				{
					RequestedPath = RequestedUrl;
				}
			}
			for (int i = 1; i < strArray.Length; i++)
			{
				index = strArray[i].IndexOf(":");
				if ((index <= 0) || (index >= (strArray[i].Length - 1)))
				{
					continue;
				}

				try
				{
					dictionary.Add(strArray[i][..index], strArray[i][(index + 1)..].Trim());
				}
				catch (Exception ex)
				{
					BaseService.Loger(ex);
				}
			}
			return dictionary;
		}

		private string RebuildQuery()
		{
			string str = $"{HttpRequestType} {RequestedPath} {HttpVersion}\r\n";
			if (HeaderFields != null)
			{
				str = HeaderFields.Keys.Aggregate(str, (current, s) => $"{current}{s.Replace("Proxy-", "")}: {HeaderFields[s]}\r\n");
				str += "\r\n";
				if (_mHttpPost != null)
				{
					str += _mHttpPost;
				}
			}
			return str;
		}

		private void SendLocalFile(string? localFile, string? requestRange, string? connection)
		{
			_mLocalFile = new LocalFile(localFile!);
			string responseStr = BuildResponse(requestRange!, connection!, out long startRange);
			_mLocalFile.FileStream!.Seek(startRange, SeekOrigin.Begin);
			try
			{
				ClientSocket?.BeginSend(Encoding.ASCII.GetBytes(responseStr), 0, responseStr.Length, SocketFlags.None, OnLocalFileSent, ClientSocket);
			}
			catch (Exception ex)
			{
				BaseService.Loger(ex);
				_mLocalFile.FileStream.Close();
				Dispose();
			}
		}

		private string BuildResponse(string requestRange, string connection, out long startRange)
		{
			string status = "200 OK";
			startRange = 0;
			long endRange = _mLocalFile!.Filesize - 1;

			StringBuilder response = new("HTTP/1.1 {0}\r\n");
			response.Append("Server: Apache\r\n");
			response.Append("Accept-Ranges: bytes\r\n");
			response.Append("Cache-Control: max-age=3600\r\n");
			response.Append("Content-Type: application/octet-stream\r\n");
			if (!string.IsNullOrEmpty(requestRange))
			{
				status = "206 Partial Content";
				string rangeStr = requestRange.Split('=')[1].Trim();
				List<string> temp = rangeStr.Split('-').Select(r => r.Trim()).ToList();
				startRange = long.Parse(temp[0]);
				if (!string.IsNullOrEmpty(temp[1]))
				{
					endRange = long.Parse(temp[1]);
				}
				else
				{
					rangeStr += (_mLocalFile.Filesize - 1);
				}

				rangeStr += $"/{_mLocalFile.Filesize}";

				response.Append(string.Format("Content-Range: bytes {0}\r\n", rangeStr));
			}
			response.Append("Date: {1}\r\n");
			response.Append("Last-Modified: {2}\r\n");
			response.Append("Content-Length: {3}\r\n");
			if (string.IsNullOrEmpty(connection))
			{
				connection = "close";
			}

			response.Append(string.Format("Connection: {0}\r\n\r\n", connection));

			string responseStr = string.Format(response.ToString(), status, DateTime.Now.ToUniversalTime().ToString("r"), _mLocalFile.LastModified.ToUniversalTime().ToString("r"), endRange + 1 - startRange);
			return responseStr;
		}

		private void SendBadRequest()
		{
			const string s =
				"HTTP/1.1 400 Bad Request\r\nConnection: close\r\nContent-Type: text/html\r\n\r\nBad Request";
			try
			{
				ClientSocket?.BeginSend(Encoding.ASCII.GetBytes(s), 0, s.Length, SocketFlags.None, OnErrorSent, ClientSocket);
			}
			catch (Exception ex)
			{
				BaseService.Loger(ex);
				Dispose();
			}
		}

		private void Blocking()
		{
			const string s =
				"HTTP/1.1 400 Bad Request\r\nConnection: close\r\nContent-Type: text/html\r\n\r\nBad Request";
			try
			{
				ClientSocket?.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		private bool IsValidQuery(string query)
		{
			if (string.IsNullOrEmpty(query))
			{
				return false;
			}

			HeaderFields = ParseQuery(query);
			return !HttpRequestType!.ToUpper().Equals("POST") || !HeaderFields.ContainsKey("Content-Length");
		}

		private void OnQuerySent(IAsyncResult ar)
		{
			try
			{
				if (DestinationSocket != null && DestinationSocket.EndSend(ar) == -1)
				{
					Dispose();
				}
				else
				{
					StartRelay();
				}
			}
			catch (Exception ex)
			{
				BaseService.Loger(ex);
				Dispose();
			}
		}

		private async void OnReceiveQuery(IAsyncResult ar)
		{
			int num = -1;
			try
			{
				if (ClientSocket != null)
				{
					num = ClientSocket.EndReceive(ar);
				}
			}
			catch (Exception ex)
			{
				BaseService.Loger(ex);
				num = -1;
			}
			if (num <= 0)
			{
				Dispose();
			}
			else
			{
				HttpQuery += Encoding.ASCII.GetString(Buffer, 0, num);
				if (IsValidQuery(HttpQuery))
				{
					await QueryHandle(HttpQuery);
				}
				else
				{
					try
					{
						ClientSocket?.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, OnReceiveQuery, ClientSocket);
					}
					catch (Exception ex)
					{
						BaseService.Loger(ex);
						Dispose();
					}
				}
			}
		}

		private void OnConnected(IAsyncResult ar)
		{
			try
			{
				if (DestinationSocket == null)
				{
					return;
				}

				string str;
				DestinationSocket.EndConnect(ar);
				if (HttpRequestType!.ToUpper().Equals("CONNECT"))
				{
					if (ClientSocket != null)
					{
						str = $"{HttpVersion} 200 Connection established\r\n\r\n";
						ClientSocket.BeginSend(Encoding.ASCII.GetBytes(str), 0, str.Length, SocketFlags.None, OnOkSent, ClientSocket);
					}
				}
				else
				{
					str = RebuildQuery();
					DestinationSocket.BeginSend(Encoding.ASCII.GetBytes(str), 0, str.Length, SocketFlags.None, OnQuerySent, DestinationSocket);
				}
			}
			catch (Exception ex)
			{
				BaseService.Loger(ex);
				Dispose();
			}
		}

		private void OnOkSent(IAsyncResult ar)
		{
			try
			{
				if (ClientSocket != null && ClientSocket.EndSend(ar) == -1)
				{
					Dispose();
				}
				else
				{
					StartRelay();
				}
			}
			catch (Exception ex)
			{
				BaseService.Loger(ex);
				Dispose();
			}
		}

		private void OnErrorSent(IAsyncResult ar)
		{
			try
			{
				ClientSocket?.EndSend(ar);
			}
			catch (Exception ex)
			{
				BaseService.Loger(ex);
				Dispose();
			}
		}

		private void OnLocalFileSent(IAsyncResult ar)
		{
			try
			{
				Socket socket = (Socket)ar.AsyncState!;
				socket.EndSend(ar);
				SendLocalFile(socket);
			}
			catch (Exception ex)
			{
				BaseService.Loger(ex);
				_mLocalFile?.FileStream?.Close();
				Dispose();
			}
		}

		private async void SendLocalFile(Socket socket)
		{
			int bufferSize = 512 * 1024;
			byte[] buffer = new byte[bufferSize];
			int bytesRead = await Task.Run(() => _mLocalFile!.FileStream!.Read(buffer, 0, bufferSize));
			if (bytesRead > 0)
			{
				try
				{
					await Task.Run(() => socket.BeginSend(buffer, 0, bytesRead, SocketFlags.None, OnLocalFileSent, socket));
				}
				catch (Exception ex)
				{
					BaseService.Loger(ex);
					_mLocalFile?.FileStream?.Close();
					Dispose();
				}
			}
			else
			{
				_mLocalFile?.FileStream?.Close();
				Dispose();
			}
		}

		public override void StartHandshake()
		{
			try
			{
				ClientSocket?.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, OnReceiveQuery, ClientSocket);
			}
			catch (Exception ex)
			{
				BaseService.Loger(ex);
				Dispose();
			}
		}

		public override string ToString()
		{
			return ToString(false);
		}

		public string ToString(bool withUrl)
		{
			string str;
			try
			{
				if (withUrl)
				{
					return string.Empty;
				}
				if ((DestinationSocket == null) || (DestinationSocket.RemoteEndPoint == null))
				{
					str = $"Incoming HTTP connection from {((IPEndPoint)ClientSocket?.RemoteEndPoint!).Address}";
				}
				else
				{
					str = $"HTTP connection from {((IPEndPoint)ClientSocket?.RemoteEndPoint!).Address} to {((IPEndPoint)DestinationSocket.RemoteEndPoint).Address} on port {((IPEndPoint)DestinationSocket.RemoteEndPoint).Port.ToString(CultureInfo.InvariantCulture)}";
				}
				if ((HeaderFields != null) && HeaderFields.ContainsKey("Host") && (RequestedPath != null))
				{
					str += $"\r\n requested URL: http://{HeaderFields["Host"]}{RequestedPath}";
				}
			}
			catch (Exception ex)
			{
				BaseService.Loger(ex);
				str = "HTTP Connection";
			}
			return str;
		}
	}
}
