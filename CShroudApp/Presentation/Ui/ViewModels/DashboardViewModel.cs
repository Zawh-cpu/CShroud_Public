using System.Collections.ObjectModel;
using System.Windows.Input;
using Ardalis.Result;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Core.Configs;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;
using Microsoft.Extensions.Options;

namespace CShroudApp.Presentation.Ui.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly ISessionManager _sessionManager;
    private readonly IVpnService _vpnService;
    private readonly IStorageManager _storageManager;
    private readonly ApplicationConfig _config;
    private readonly ILocalizationService _localizationService;
    private readonly IApiRepository _apiRepository;
    private readonly INotificationManager _notificationManager;
    
    public DateTime Start = DateTime.UtcNow;
    private DispatcherTimer _timer;
    
    [ObservableProperty]
    private string _timerText = "00:00:00";
    
    public class ModeItem
    {
        public string Name { get; set; }
        public int? HttpPort { get; set; }
        public int? SocksPort { get; set; }
    }
    
    public ObservableCollection<KeyValuePair<VpnMode, ModeItem>> Modes { get; } = new();
    
    public IEnumerable<ModeItem> ModesForView => Modes.Select(x => x.Value);
    
    public ICommand ToggleVpnCommand { get; }

    public string CurrentIpAddress { get; set; } = "91.144.254.24";
    
    public DashboardViewModel(ISessionManager sessionManager, IVpnService vpnService, ApplicationConfig config, IStorageManager storageManager, ILocalizationService localizationService, IApiRepository apiRepository, INotificationManager notificationManager)
    {
        _sessionManager = sessionManager;
        _vpnService = vpnService;
        _config = config;
        _storageManager = storageManager;
        _localizationService = localizationService;
        _apiRepository = apiRepository;
        _notificationManager = notificationManager;
        
        Modes.Add(new KeyValuePair<VpnMode, ModeItem>(VpnMode.Tun, new ModeItem { Name = _localizationService.Translate("VpnMode-Tun"), HttpPort = null, SocksPort = null }));
        Modes.Add(new KeyValuePair<VpnMode, ModeItem>(VpnMode.TunPlusProxy, new ModeItem { Name = _localizationService.Translate("VpnMode-TunPlusProxy"), HttpPort = (int)_config.Vpn.Inputs.Http.Port, SocksPort = (int)_config.Vpn.Inputs.Socks.Port }));
        Modes.Add(new KeyValuePair<VpnMode, ModeItem>(VpnMode.Proxy, new ModeItem { Name = _localizationService.Translate("VpnMode-Proxy"), HttpPort = (int)_config.Vpn.Inputs.Http.Port, SocksPort = (int)_config.Vpn.Inputs.Socks.Port }));
        Modes.Add(new KeyValuePair<VpnMode, ModeItem>(VpnMode.TransparentProxy, new ModeItem { Name = _localizationService.Translate("VpnMode-TransparentProxy"), HttpPort = (int)_config.Vpn.Inputs.Http.Port, SocksPort = (int)_config.Vpn.Inputs.Socks.Port }));
        
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        
        Console.WriteLine(_sessionManager.Session.Nickname);
        ToggleVpnCommand = new RelayCommand(() => Task.Run(ToggleVpn));

        SelectedMode = Modes.FirstOrDefault(x => x.Key == _config.Vpn.Mode).Value ?? Modes.FirstOrDefault(x => x.Key == VpnMode.Tun).Value;
        
        _vpnService.VpnEnabled += OnVpnEnabled;
        _vpnService.VpnDisabled += OnVpnDisabled;
        _vpnService.VpnStartedCancellation += OnVpnStartedCancellation;
    }

    public async Task ToggleVpn()
    {
        if (_vpnService.IsRunning)
        {
            await _vpnService.DisableAsync();
        }
        else
        {
            VpnConnectionCredentials? credentials = _storageManager.GetValue<VpnConnectionCredentials>("VpnConnectionCredentials");
            if (credentials is null)
            {
                var temp = await _apiRepository.TryConnectToVpnNetworkAsync(_vpnService.SupportedProtocols,
                    "frankfurt");
                if (temp.IsSuccess) credentials = temp.Value;
            }
            
            if (credentials is null)
            {
                _notificationManager.AddNotification(new NotificationObject()
                {
                    Title = _localizationService.Translate("VpnNetwork-ErrorConnection"),
                    Message = _localizationService.Translate("VpnNetwork-ErrorConnection-Text"),
                    Type = NotificationType.Error
                });
                
                return;
            }

            await _vpnService.EnableAsync(_config.Vpn.Mode, credentials);
        }
    }

    private void OnVpnEnabled(object? sender, EventArgs e)
    {
        Console.WriteLine("Connection started");
        Start = DateTime.UtcNow;
        _timer.Start();
    }

    private void OnVpnDisabled(object? sender, EventArgs e)
    {
        Console.WriteLine("Connection closed");
        _timer.Stop();
        TimerText = "00:00:00";
    }

    private void OnVpnStartedCancellation(Result<object> result)
    {
        switch (result.Status)
        {
            case ResultStatus.Unauthorized:
                _notificationManager.AddNotification(new NotificationObject()
                {
                    Title = _localizationService.Translate("Error-InsufficientRights"),
                    Message = _localizationService.Translate("VpnService-AdminRightsRequired-Text"),
                    Type = NotificationType.Error
                });
                break;
        }
    }
    
    private void Timer_Tick(object? sender, EventArgs e)
    {
        var remaining = DateTime.UtcNow - Start;

        if (remaining <= TimeSpan.Zero)
        {
            TimerText = "00:00:00";
            _timer.Stop();
        }
        else
        {
            TimerText = $"{remaining.Hours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
        }
    }
    
    [ObservableProperty]
    private ModeItem? _selectedMode;

    partial void OnSelectedModeChanged(ModeItem? value)
    {
        if (value is null) return;
        
        var key = Modes.FirstOrDefault(x => x.Value.Name == value.Name).Key;

        _config.Vpn.Mode = key;
        //Task.Run(_storageManager.SaveConfigAsync);
        //if (_vpnService.IsRunning)
        //    Task.Run(async() => await _vpnService.RestartAsync(_settingsConfig.Network.Mode));
    }
}