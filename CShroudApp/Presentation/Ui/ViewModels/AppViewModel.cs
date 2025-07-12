using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Core.Configs;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.StaticServices;

namespace CShroudApp.Presentation.Ui.ViewModels;

public partial class AppViewModel : ViewModelBase
{
    private readonly IVpnService _vpnService;
    private readonly IConfigManager _configManager;
    private readonly ApplicationConfig _config;
    private readonly IStorageManager _storageManager;
    private readonly IApiRepository _apiRepository;
    
    [ObservableProperty]
    private string _vpnTogglerText = null!;
    
    
    public AppViewModel(IVpnService vpnService, IConfigManager configManager, ApplicationConfig applicationConfig, IStorageManager storageManager, IApiRepository apiRepository)
    {
        _vpnService = vpnService;
        _configManager = configManager;
        _config = applicationConfig;
        _storageManager = storageManager;
        _apiRepository = apiRepository;
        _configManager.ConfigChanged += OnUpdateNeeds;
        _vpnService.VpnDisabled += OnUpdateEventPlugin;
        _vpnService.VpnEnabled += OnUpdateEventPlugin;
        _vpnService.VpnStartedCancellation += OnUpdateActionPlugin;
        
        OnUpdateNeeds();
        //NativeMenuItems.Add(new NativeMenuItem { Header = string.Format(_vpnService.IsRunning ? _localizationService.Translate("NativeMenu-Disable") : _localizationService.Translate("NativeMenu-Enable"), _localizationService.Translate($"VpnMode-{_config.Vpn.Mode.ToString()}")), Command = ShowCommand });
    }

    public void OnUpdateEventPlugin(object? sender = null, EventArgs? args = null) => OnUpdateNeeds();

    public void OnUpdateActionPlugin(object? trash) => OnUpdateNeeds();

    private void OnUpdateNeeds()
    {
        VpnTogglerText = string.Format(_vpnService.IsRunning ? LocalizationService.Translate("Tray-Ui-Disable") : LocalizationService.Translate("Tray-Ui-Enable"), LocalizationService.Translate($"VpnMode-{_config.Vpn.Mode.ToString()}"));
    }
    
    [RelayCommand]
    private void Show()
    {
        if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var window = desktop.MainWindow;

            if (window is null)
                return;

            if (!window.IsVisible)
            {
                window.Show();
                window.WindowState = WindowState.Normal;
                window.Activate();
            }
            else if (window.WindowState == WindowState.Minimized)
            {
                window.WindowState = WindowState.Normal;
                window.Activate();
            }
        }
    }

    [RelayCommand]
    private void Exit()
    {
        if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
    
    [RelayCommand]
    private void ToggleVpn()
    {
        Task.Run(async () =>
        {
            if (_vpnService.IsRunning)
                await _vpnService.DisableAsync();
            else
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
    }

    [RelayCommand]
    private void Hide()
    {
        if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow?.Hide();
    }
    
    [RelayCommand]
    private void SetMode(VpnMode mode)
    {
        _config.Vpn.Mode = mode;
        Task.Run(async () =>
        {
            _ = _configManager.SaveConfigAsync();
            _configManager.NotifyConfigChanged();
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
    }
}