using Ardalis.Result;
using CShroudApp.Core.Configs;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class VpnService : IVpnService
{
    public bool IsRunning => _vpnCore.IsRunning;
    
    public event EventHandler? VpnEnabled;
    public event EventHandler? VpnDisabled;
    public event Action<Result<object>>? VpnStartedCancellation;
    
    private readonly IVpnCore _vpnCore;
    private readonly IProxyManager _proxyManager;
    private readonly ApplicationConfig _applicationConfig;
    
    public VpnProtocol[] SupportedProtocols => _vpnCore.SupportedProtocols;

    private VpnConfig? _savedCurrentSessionConfig;
    private VpnMode? _currentEnabledMode;

    public VpnService(IVpnCore vpnCore, IProxyManager proxyManager, ApplicationConfig applicationConfig)
    {
        _vpnCore = vpnCore;
        _proxyManager = proxyManager;
        _applicationConfig = applicationConfig;

        _vpnCore.CoreEnabled += OnCoreEnabled;
        _vpnCore.CoreDisabled += OnCoreDisabled;
    }
    
    public async Task<Result> EnableAsync(VpnMode mode, VpnConnectionCredentials credentials)
    {
        if (mode == VpnMode.Tun && _vpnCore.DoNeedElevationForTun)
        {
            Console.WriteLine("NEEDS TO REQUEST A RIGHTS ELEVATION TO ENABLE TUN.");
            VpnStartedCancellation?.Invoke(Result.Unauthorized());
            return Result.Unauthorized();
        }
        
        _savedCurrentSessionConfig = _applicationConfig.Vpn;
        _savedCurrentSessionConfig.Mode = mode;
        _currentEnabledMode = mode;
        
        if (IsRunning) return Result.Conflict();
        if (!SupportedProtocols.Contains(credentials.Protocol)) return Result.Invalid();
        
        return (await _vpnCore.EnableAsync(mode, credentials)).Map();
    }

    public async Task DisableAsync()
    {
        switch (_currentEnabledMode)
        {
            case VpnMode.Proxy:
                if (_applicationConfig.Vpn.SavePreviousProxy && _proxyManager.CachedProxy is not null)
                    _proxyManager.SetupProxy(_proxyManager.CachedProxy);
                else
                    _proxyManager.DisableProxy();
                break;
            
            case VpnMode.Tun or VpnMode.TunPlusProxy:
                if (!_vpnCore.AutoSetInboundSupportedProtocol.Contains(VpnProtocol.Tun))
                    Console.WriteLine("Needs to disable Tun");
                break;
        }
        
        _savedCurrentSessionConfig = null;
        _currentEnabledMode = null;
        
        await _vpnCore.DisableAsync();
    }

    public async Task RestartAsync(VpnMode mode, VpnConnectionCredentials credentials)
    {
        await DisableAsync();
        await EnableAsync(mode, credentials);
    }

    private void OnCoreEnabled(object? sender, EventArgs e)
    {
        if (_savedCurrentSessionConfig is not null)
        {
            switch (_savedCurrentSessionConfig.Mode)
            {
                case VpnMode.Proxy:
                    if (_savedCurrentSessionConfig.SavePreviousProxy)
                        _proxyManager.CachedProxy = _proxyManager.GetProxyData();
                    
                    if (_savedCurrentSessionConfig.Inputs.PreferredInput == ProxyProtocol.Socks)
                        _proxyManager.SetupProxy(
                            new ProxyData(ProxyProtocol.Socks,
                                _savedCurrentSessionConfig.Inputs.Socks.Host,
                                _savedCurrentSessionConfig.Inputs.Socks.Port,
                                _savedCurrentSessionConfig.Inputs.ExcludeProxyForAddresses, true));
                    else
                        _proxyManager.SetupProxy(
                            new ProxyData(ProxyProtocol.Http,
                                _savedCurrentSessionConfig.Inputs.Http.Host,
                                _savedCurrentSessionConfig.Inputs.Http.Port,
                                _savedCurrentSessionConfig.Inputs.ExcludeProxyForAddresses, true));
                    break;
                
                case VpnMode.Tun or VpnMode.TunPlusProxy:
                    Console.WriteLine("FWEFWEFFWEF ");
                    if (!_vpnCore.AutoSetInboundSupportedProtocol.Contains(VpnProtocol.Tun))
                        Console.WriteLine("Needs to enable Tun");
                    break;
            }
            
            _savedCurrentSessionConfig = null;
        }
        VpnEnabled?.Invoke(this, e);
    }
    
    private void OnCoreDisabled(object? sender, EventArgs e)
    {
        VpnDisabled?.Invoke(this, e);
    }
}