using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace PSXhub.Application.Services
{
	public static class FileService
	{
		public static string ComputeSHA1(string filePath)
		{
			using (FileStream fs = File.OpenRead(filePath))
			{
				SHA1 sha1 = SHA1.Create();
				byte[] hash = sha1.ComputeHash(fs);
				return BitConverter.ToString(hash).Replace("-", "").ToLower();
			}
		}

		public static string RequestToUrl(string request)
		{
			int index = request.IndexOf('?');
			if (index >= 0)
				request = request.Substring(0, index);

			return request;
		}

		public static string QueryToUrl(string request)
		{
			try
			{
				Uri uri = new Uri(request.Split(' ')[1]);
				return uri.GetLeftPart(UriPartial.Path).TrimEnd('/');
			}
			catch (Exception ex)
			{
				return string.Empty;
			}
		}

		public static string UrlToFileName(string url)
		{
			string fileName = Path.GetFileName(new Uri(url).AbsolutePath);

			return fileName;
		}

		public static bool CheckIsFile(string request)
		{
			string pattern = @"\.pkg$|\.pup|\.json";
			return Regex.IsMatch(request, pattern);
		}
	}
}
