using ICS;
using NETCONLib;
using PSXhub.Application.Services;
using PSXhub.Localization;
using PSXhub.NetworkSharing.WPF.Models;
using PSXhub.NetworkSharing.WPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using RelayCommand = PSXhub.Localization.RelayCommand;

public class NetworkSharingViewModel : ViewModelBase
{
	private readonly List<INetConnection> _allInterfaces;

	public ICollectionView EthernetAdapterView { get; }
	public ICollectionView PSXAdapterView { get; }

	private NetworkAdapterItem _ethernetAdapter;
	public NetworkAdapterItem EthernetAdapter
	{
		get => _ethernetAdapter;
		set
		{
			if (_ethernetAdapter != value)
			{
				_ethernetAdapter = value;
				OnPropertyChanged(nameof(EthernetAdapter));
				OnPropertyChanged(nameof(IsEnabledStartButton));
				OnPropertyChanged(nameof(WarningOpacity));
				PSXAdapterView.Refresh();
			}
		}
	}

	private NetworkAdapterItem _pSXAdapter;
	public NetworkAdapterItem PSXAdapter
	{
		get => _pSXAdapter;
		set
		{
			if (_pSXAdapter != value)
			{
				_pSXAdapter = value;
				OnPropertyChanged(nameof(PSXAdapter));
				OnPropertyChanged(nameof(IsEnabledStartButton));
				OnPropertyChanged(nameof(WarningOpacity));
				EthernetAdapterView.Refresh();
			}
		}
	}

	private bool _activeSharing = false;
	public bool ActiveSharing
	{
		get => _activeSharing;
		set
		{
			_activeSharing = value;
			OnPropertyChanged(nameof(ActiveSharing));
			OnPropertyChanged(nameof(StartButtonVisibility));
			OnPropertyChanged(nameof(StopButtonVisibility));
			OnPropertyChanged(nameof(ComboBoxesActivity));
		}
	}

	public bool ComboBoxesActivity => !ActiveSharing;

	public Visibility StartButtonVisibility =>
		!ActiveSharing ? Visibility.Visible : Visibility.Collapsed;

	public Visibility StopButtonVisibility =>
	ActiveSharing ? Visibility.Visible : Visibility.Collapsed;

	public bool IsEnabledStartButton =>
		EthernetAdapter.DisplayName != T("NS10")
		&& EthernetAdapter != null
		&& PSXAdapter.DisplayName != T("NS10")
		&& PSXAdapter != null;

	public double WarningOpacity =>
		EthernetAdapter.DisplayName != T("NS10")
		&& EthernetAdapter != null
		&& PSXAdapter.DisplayName != T("NS10")
		&& PSXAdapter != null ? 1 : 0.5;

	private bool _starting = false;
	public bool Starting
	{
		get => _starting;
		set
		{
			_starting = value;
			OnPropertyChanged(nameof(Starting));
			OnPropertyChanged(nameof(TextStartButtonVisibility));
			OnPropertyChanged(nameof(LoadingStartButtonVisibility));
		}
	}

	public Visibility TextStartButtonVisibility =>
		!Starting ? Visibility.Visible : Visibility.Collapsed;

	public Visibility LoadingStartButtonVisibility =>
		Starting ? Visibility.Visible : Visibility.Collapsed;

	public ICommand StartCommand => new RelayCommand(async () => await Start());

	private async Task Start()
	{
		Starting = true;

		try
		{

			// await Task.Run(() =>
			// {
			// 	var staThread = new Thread(() =>
			// 	{
			// 		try
			// 		{
			// 			var ethernet = EthernetAdapter.Interface;
			// 			var psx = PSXAdapter.Interface;
			//
			// 			ICS2.Manager2.IcsManager.ShareConnection(ethernet, psx);
			//
			// 			Settings.EthernetAdapter = Manager.PrintConnectionProperties(EthernetAdapter.Interface).Guid;
			// 			Settings.PSXAdapter = Manager.PrintConnectionProperties(PSXAdapter.Interface).Guid;
			// 		}
			// 		catch (Exception ex)
			// 		{
			// 			AdapterService.RestartICSService();
			// 		}
			// 	});
			//
			// 	staThread.SetApartmentState(ApartmentState.STA);
			// 	staThread.Start();
			// 	staThread.Join();
			// });

			var ethernet = EthernetAdapter.Interface;
			var psx = PSXAdapter.Interface;

			await Task.Run(() =>
			{
				ICS2.Manager2.IcsManager.ShareConnection(ethernet, psx);
			});

			Settings.EthernetAdapter = Manager.PrintConnectionProperties(ethernet).Guid;
			Settings.PSXAdapter = Manager.PrintConnectionProperties(psx).Guid;

			await Task.Run(() => CheckEnableSharing());

			Starting = false;

			SettingsManager.Instance.SaveSettings();
		}
		catch
		{
			IcsServiceManager.RestartICSService();
		}
	}


	private bool _stopping = false;
	public bool Stopping
	{
		get => _stopping;
		set
		{
			_stopping = value;
			OnPropertyChanged(nameof(Stopping));
			OnPropertyChanged(nameof(TextStopButtonVisibility));
			OnPropertyChanged(nameof(LoadingStopButtonVisibility));
		}
	}

	public Visibility TextStopButtonVisibility =>
		!Stopping ? Visibility.Visible : Visibility.Collapsed;

	public Visibility LoadingStopButtonVisibility =>
		Stopping ? Visibility.Visible : Visibility.Collapsed;


	public ICommand StopCommand => new RelayCommand(Stop);
	private async void Stop()
	{
		Stopping = true;
		try
		{
			// await Task.Run(() => Manager.DisableInternetSharing(EthernetAdapter.Interface));
			// await Task.Run(() => AdapterService.SetAdapterToDhcpByGuid(Manager.PrintConnectionProperties(PSXAdapter.Interface).Guid));

			await Task.Run(() =>
			{
				IcsServiceManager.StopSharing();
			});
			await Task.Run(() => IcsServiceManager.SetAdapterToDhcpByGuid(Manager.PrintConnectionProperties(PSXAdapter.Interface).Guid));

			await Task.Run(() => CheckEnableSharing());
		}
		catch (Exception ex)
		{
		}
		finally
		{
			Stopping = false;
		}
	}

	private ObservableCollection<NetworkAdapterItem> _ethernetList;
	private ObservableCollection<NetworkAdapterItem> _psxList;

	public NetworkSharingViewModel()
	{
		_allInterfaces = Manager.GetActiveNetworkConnections();

		_ethernetList = new ObservableCollection<NetworkAdapterItem>();
		_psxList = new ObservableCollection<NetworkAdapterItem>();

		_ethernetList.Add(new NetworkAdapterItem(T("NS10"), null));
		_psxList.Add(new NetworkAdapterItem(T("NS10"), null));

		foreach (var adapter in _allInterfaces)
		{
			_ethernetList.Add(new NetworkAdapterItem(Manager.PrintConnectionProperties(adapter).Name, adapter));
			_psxList.Add(new NetworkAdapterItem(Manager.PrintConnectionProperties(adapter).Name, adapter));
		}

		EthernetAdapterView = CollectionViewSource.GetDefaultView(_ethernetList);
		EthernetAdapterView.Filter = EthernetAdapterFilter;

		PSXAdapterView = CollectionViewSource.GetDefaultView(_psxList);
		PSXAdapterView.Filter = PSXAdapterFilter;

		EthernetAdapter =
			_ethernetList
				.FirstOrDefault(w =>
					w.Interface != null && Manager.PrintConnectionProperties(w.Interface).Guid != null &&
					Manager.PrintConnectionProperties(w.Interface).Guid == Settings.EthernetAdapter)
			?? _ethernetList.First();

		PSXAdapter =
			_psxList
				.FirstOrDefault(w =>
					w.Interface != null
					&& Manager.PrintConnectionProperties(w.Interface).Guid != null
					&& Manager.PrintConnectionProperties(w.Interface).Guid == Settings.PSXAdapter)
			?? _psxList.First();

		CheckEnableSharing();
	}

	private bool EthernetAdapterFilter(object obj)
	{
		if (obj is not NetworkAdapterItem item)
			return false;

		if (item.Interface == null)
			return true;

		var itemGuid = SafeGetGuid(item.Interface);
		var psxGuid = SafeGetGuid(PSXAdapter?.Interface);

		if (psxGuid.HasValue && itemGuid.HasValue)
			return itemGuid != psxGuid;

		return true;
	}


	private bool PSXAdapterFilter(object obj)
	{
		if (obj is not NetworkAdapterItem item)
			return false;

		if (item.Interface == null)
			return true;

		var itemGuid = SafeGetGuid(item.Interface);
		var ethernetGuid = SafeGetGuid(EthernetAdapter?.Interface);

		if (ethernetGuid.HasValue && itemGuid.HasValue)
			return itemGuid != ethernetGuid;

		return true;
	}


	private Guid? SafeGetGuid(INetConnection conn)
	{
		try
		{
			var props = Manager.PrintConnectionProperties(conn);
			if (Guid.TryParse(props?.Guid, out var parsedGuid))
				return parsedGuid;
		}
		catch
		{
		}
		return null;
	}

	private void CheckEnableSharing()
	{
		var checkerResponse = Manager.GetCurrentICS();

		if (checkerResponse.isSharing)
		{
			ActiveSharing = true;

			if (checkerResponse.publicAdapter != null)
			{
				EthernetAdapter =
					_ethernetList
						.FirstOrDefault(w =>
							w.Interface != null && Manager.PrintConnectionProperties(w.Interface).Guid != null &&
							Manager.PrintConnectionProperties(w.Interface).Guid ==
							Manager.PrintConnectionProperties(checkerResponse.publicAdapter).Guid)
					?? _ethernetList.First();
			}

			if (checkerResponse.privateAdapter != null)
			{
				PSXAdapter =
					_psxList
						.FirstOrDefault(w =>
							w.Interface != null
							&& Manager.PrintConnectionProperties(w.Interface).Guid != null
							&& Manager.PrintConnectionProperties(w.Interface).Guid ==
							Manager.PrintConnectionProperties(checkerResponse.privateAdapter).Guid)
					?? _psxList.First();
			}
		}
		else
		{
			ActiveSharing = false;
		}
	}
}