using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;
using Microsoft.Extensions.Options;
using Splat.ModeDetection;

namespace CShroudApp.Presentation.Ui.ViewModels;

public partial class DashBoardViewModel : ViewModelBase
{
    private readonly ISessionManager _sessionManager;
    private readonly IEventManager _eventManager;
    private readonly IVpnService _vpnService;
    private readonly SettingsConfig _settingsConfig;
    private readonly IStorageManager _storageManager;
    
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
    
    public ObservableCollection<KeyValuePair<VpnMode, ModeItem>> Modes { get; } = new()
    {
        new KeyValuePair<VpnMode, ModeItem>(VpnMode.Proxy, new ModeItem { Name = "Proxy", HttpPort = 10808, SocksPort = 10809 }),
        new KeyValuePair<VpnMode, ModeItem>(VpnMode.ProxyAndTun, new ModeItem { Name = "Proxy&Tun", HttpPort = 10808, SocksPort = 10809 }),
        new KeyValuePair<VpnMode, ModeItem>(VpnMode.Tun, new ModeItem { Name = "Full VPN", HttpPort = null, SocksPort = null }),
        new KeyValuePair<VpnMode, ModeItem>(VpnMode.Transparent, new ModeItem { Name = "Transparent", HttpPort = 10808, SocksPort = 10809 }),
    };
    
    public IEnumerable<ModeItem> ModesForView => Modes.Select(x => x.Value);
    
    public ICommand ToggleVpnCommand { get; }

    public bool temp = false;
    
    public DashBoardViewModel(ISessionManager sessionManager, IEventManager eventManager, IVpnService vpnService, IOptions<SettingsConfig> settingsConfig, IStorageManager storageManager)
    {
        _sessionManager = sessionManager;
        _eventManager = eventManager;
        _vpnService = vpnService;
        _settingsConfig = settingsConfig.Value;
        _storageManager = storageManager;
        
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        
        Console.WriteLine(_sessionManager.Session.Nickname);
        ToggleVpnCommand = new RelayCommand(() => Task.Run(ToggleVpn));

        SelectedMode = Modes.FirstOrDefault(x => x.Key == _settingsConfig.Network.Mode).Value ?? Modes.FirstOrDefault(x => x.Key == VpnMode.Tun).Value;
        Console.WriteLine(SelectedMode);
        Console.WriteLine(_settingsConfig.Network.Mode);
        
        _vpnService.VpnEnabled += OnVpnEnabled;
        _vpnService.VpnDisabled += OnVpnDisabled;
    }

    public async Task ToggleVpn()
    {
        Console.WriteLine($"IS ENABLED? = {_vpnService.IsRunning}");
        if (_vpnService.IsRunning)
        {
            Console.WriteLine("TRYING TO DISABLE VPN");
            await _vpnService.DisableAsync();
        }
        else
            await _vpnService.EnableAsync(_settingsConfig.Network.Mode);
        
        temp = !temp;
    }

    private void OnVpnEnabled(object? sender, EventArgs e)
    {
        Start = DateTime.UtcNow;
        _timer.Start();
    }

    private void OnVpnDisabled(object? sender, EventArgs e)
    {
        _timer.Stop();
        TimerText = "00:00:00";
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

        _settingsConfig.Network.Mode = key;
        Task.Run(_storageManager.SaveConfigAsync);
        if (_vpnService.IsRunning)
            Task.Run(async() => await _vpnService.RestartAsync(_settingsConfig.Network.Mode));
    }
}