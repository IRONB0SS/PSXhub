// using System.Runtime.InteropServices;
// using System.Text;
// using System.Windows;
// using System.Windows.Interop;
//
// namespace PsxDataHelper.UI.Services
// {
//     public class SHBrowse
//     {
// 	    public static class NativeMethods
// 	    {
// 		    [DllImport("shell32.dll")]
// 		    public static extern IntPtr SHBrowseForFolder(ref BROWSEINFO lpbi);
//
// 			[DllImport("shell32.dll", CharSet = CharSet.Auto)]
// 			public static extern bool SHGetPathFromIDList(IntPtr pidl, StringBuilder pszPath);
//
// 			[DllImport("shell32.dll")]
// 		    public static extern void CoTaskMemFree(IntPtr pv);
//
// 		    [StructLayout(LayoutKind.Sequential)]
// 		    public struct BROWSEINFO
// 		    {
// 			    public IntPtr hwndOwner;
// 			    public IntPtr pidlRoot;
// 			    public IntPtr pszDisplayName;
// 			    public string lpszTitle;
// 			    public uint ulFlags;
// 			    public IntPtr lpfn;
// 			    public IntPtr lParam;
// 			    public int iImage;
// 		    }
// 	    }
//
//
// 		public static string SelectFolder(string windowTitle)
// 		{
// 		 IntPtr buffer = Marshal.AllocHGlobal(260 * Marshal.SystemDefaultCharSize);
// 		 try
// 		 {
// 		  NativeMethods.BROWSEINFO bi = new NativeMethods.BROWSEINFO();
// 		  bi.hwndOwner = new WindowInteropHelper(System.Windows.Application.Current.MainWindow).Handle;
// 		  bi.lpszTitle = windowTitle;
// 		  bi.ulFlags =
// 		   0x00000040; //bi.ulFlags = 0x00000000; //bi.ulFlags = 0x00000040; // BIF_USENEWUI, Enable the new UI style
// 		
// 		  IntPtr pidl = NativeMethods.SHBrowseForFolder(ref bi);
// 		  StringBuilder path = new StringBuilder(260);
// 		
// 		  if (pidl != IntPtr.Zero)
// 		  {
// 		   bool success = NativeMethods.SHGetPathFromIDList(pidl, path);
// 		
// 		   if (success)
// 		   {
// 		    string folderPath = path.ToString();
// 		    return folderPath;
// 		   }
// 		  }
// 		 }
// 		 catch (Exception ex)
// 		 {
// 		  MessageBox.Show("Error: " + ex.Message);
// 		 }
// 		 finally
// 		 {
// 		  Marshal.FreeHGlobal(buffer);
// 		 }
// 		
// 		 return null;
// 		}
// 	}
// }