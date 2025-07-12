using System.Collections.ObjectModel;
using System.Windows.Input;
using Ardalis.Result;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Core.Configs;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.StaticServices;

namespace CShroudApp.Presentation.Ui.ViewModels;

public partial class DashboardViewModel : ViewModelBase, IDisposable
{
    private readonly ISessionManager _sessionManager;
    private readonly IVpnService _vpnService;
    private readonly IStorageManager _storageManager;
    private readonly ApplicationConfig _config;
    private readonly IApiRepository _apiRepository;
    private readonly INotificationManager _notificationManager;
    private readonly IConfigManager _configManager;
    private readonly IToastManager _toastManager;
    
    public DateTime Start = DateTime.UtcNow;
    private DispatcherTimer _timer;
    
    [ObservableProperty]
    private string _timerText = "00:00:00";
    
    public class ModeItem
    {
        public required string Name { get; set; }
        public int? HttpPort { get; set; }
        public int? SocksPort { get; set; }
    }
    
    public ObservableCollection<KeyValuePair<VpnMode, ModeItem>> Modes { get; } = new();
    
    public IEnumerable<ModeItem> ModesForView => Modes.Select(x => x.Value);
    
    public ICommand ToggleVpnCommand { get; }

    public string CurrentIpAddress { get; set; } = "91.144.254.24";
    
    public DashboardViewModel(ISessionManager sessionManager, IVpnService vpnService, ApplicationConfig config, IStorageManager storageManager, IApiRepository apiRepository, INotificationManager notificationManager, IConfigManager configManager, IToastManager toastManager)
    {
        _sessionManager = sessionManager;
        _vpnService = vpnService;
        _config = config;
        _storageManager = storageManager;
        _apiRepository = apiRepository;
        _notificationManager = notificationManager;
        _configManager = configManager;
        _toastManager = toastManager;
        
        Modes.Add(new KeyValuePair<VpnMode, ModeItem>(VpnMode.Tun, new ModeItem { Name = LocalizationService.Translate("VpnMode-Tun"), HttpPort = null, SocksPort = null }));
        Modes.Add(new KeyValuePair<VpnMode, ModeItem>(VpnMode.TunPlusProxy, new ModeItem { Name = LocalizationService.Translate("VpnMode-TunPlusProxy"), HttpPort = (int)_config.Vpn.Inputs.Http.Port, SocksPort = (int)_config.Vpn.Inputs.Socks.Port }));
        Modes.Add(new KeyValuePair<VpnMode, ModeItem>(VpnMode.Proxy, new ModeItem { Name = LocalizationService.Translate("VpnMode-Proxy"), HttpPort = (int)_config.Vpn.Inputs.Http.Port, SocksPort = (int)_config.Vpn.Inputs.Socks.Port }));
        Modes.Add(new KeyValuePair<VpnMode, ModeItem>(VpnMode.TransparentProxy, new ModeItem { Name = LocalizationService.Translate("VpnMode-TransparentProxy"), HttpPort = (int)_config.Vpn.Inputs.Http.Port, SocksPort = (int)_config.Vpn.Inputs.Socks.Port }));
        
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        
        ToggleVpnCommand = new RelayCommand(() => Task.Run(ToggleVpn));

        SelectedMode = Modes.FirstOrDefault(x => x.Key == _config.Vpn.Mode).Value ?? Modes.FirstOrDefault(x => x.Key == VpnMode.Tun).Value;
        
        _vpnService.VpnEnabled += OnVpnEnabled;
        _vpnService.VpnDisabled += OnVpnDisabled;
        _vpnService.VpnStartedCancellation += OnVpnStartedCancellation;
        _configManager.ConfigChanged += OnConfigChanged;
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
                    Title = LocalizationService.Translate("VpnNetwork-ErrorConnection"),
                    Message = LocalizationService.Translate("VpnNetwork-ErrorConnection-Text"),
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
        _toastManager.SendToast();
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
                    Title = LocalizationService.Translate("Error-InsufficientRights"),
                    Message = LocalizationService.Translate("VpnService-AdminRightsRequired-Text"),
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
        Task.Run(async () =>
        {
            _ = _configManager.SaveConfigAsync();
            if (_vpnService.IsRunning)
            {
                VpnConnectionCredentials? credentials = _storageManager.GetValue<VpnConnectionCredentials>("VpnConnectionCredentials");
                if (credentials is null)
                {
                    var temp = await _apiRepository.TryConnectToVpnNetworkAsync(_vpnService.SupportedProtocols,
                        "frankfurt");
                    if (temp.IsSuccess) credentials = temp.Value;
                }

                if (credentials is not null)
                    await _vpnService.RestartAsync(_config.Vpn.Mode, credentials);

            }
        });
        
        _configManager.NotifyConfigChanged();
    }

    public void OnConfigChanged()
    {
        var mode = Modes.FirstOrDefault(x => x.Key == _config.Vpn.Mode).Value;
        if (mode is null) return;

        SelectedMode = mode;
    }
    
    public void Dispose()
    {
        _vpnService.VpnEnabled -= OnVpnEnabled;
        _vpnService.VpnDisabled -= OnVpnDisabled;
        _vpnService.VpnStartedCancellation -= OnVpnStartedCancellation;
        _configManager.ConfigChanged -= OnConfigChanged;
    }
}