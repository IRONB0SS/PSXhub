using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows;
using PsxDataHelper.Application;
using PsxDataHelper.Application.Model;
using PsxDataHelper.Application.Services;

namespace PsxDataHelper
{
	public partial class App : System.Windows.Application
    {
		public AppSettings Settings => SettingsManager.Instance.AppSettings;
		
		protected override void OnStartup(StartupEventArgs e)
	    {
		    base.OnStartup(e);

			LogService.ClearLogs();

		    this.ShutdownMode = ShutdownMode.OnMainWindowClose;
		   
			var identity = WindowsIdentity.GetCurrent();
		    var principal = new WindowsPrincipal(identity);
		    var isadmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

		    if (isadmin)
		    {
			    if (Settings.IsLocalAdmin)
			    {
				    Settings.IsLocalAdmin = false;
				    SettingsManager.Instance.SaveSettings();

					if (Settings.IsAdminNetworkSharingWindow)
					{
						Settings.IsAdminNetworkSharingWindow = false;
						SettingsManager.Instance.SaveSettings();
						if (NetworkSharing.IsAdminWindowRunning())
						{
							System.Windows.Application.Current.Shutdown();
							return;
						}
						else
						{
							NetworkSharing networkSharing = new NetworkSharing();
							networkSharing.ShowDialog();
						}
					}

					else
					{
						return;
					}
				}
		    }
		    // else
		    // {
			   //  App.Current.Shutdown();
		    // }
	    }
	}
}
