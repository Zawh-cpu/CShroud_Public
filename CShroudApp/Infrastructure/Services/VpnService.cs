using CShroudApp.Application.Factories;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;
using Microsoft.Extensions.Options;

namespace CShroudApp.Infrastructure.Services;

public class VpnService : IVpnService
{
    public event EventHandler? VpnEnabled;
    public event EventHandler? VpnDisabled;
    
    private readonly SettingsConfig _settingsConfig;
    
    private readonly IVpnCore _vpnCore;
    private readonly IApiRepository _apiRepository;
    private readonly IProxyManager _proxyManager;

    public VpnMode CurrentMode { get; private set; } = VpnMode.Disabled;
    
    public bool IsRunning => _vpnCore.IsRunning;


    public VpnService(IOptions<SettingsConfig> settingsConfig,
        IVpnCore vpnCore, IApiRepository apiRepository, IProxyManager proxyManager)
    {
        _settingsConfig = settingsConfig.Value;
        _vpnCore = vpnCore;
        _apiRepository = apiRepository;
        _proxyManager = proxyManager;
        
        _vpnCore.VpnEnabled += OnVpnEnabled;
        _vpnCore.VpnDisabled += OnVpnDisabled;
    }
    

    public async Task EnableAsync(VpnMode mode)
    {
        var credentials = await _apiRepository.ConnectToVpnNetworkAsync(_vpnCore.SupportedProtocols, "de-frankfurt");
        if (credentials is null || !_vpnCore.IsSupportProtocol(credentials.Protocol))
        {
            throw new NotSupportedException($"{(credentials is null ? "UNKNOWN" : credentials.Protocol.ToString())} is unsupported protocol");
        }
        
        var outbound = IVpnBoundFactory.CreateFromCredentials(credentials);
        _vpnCore.ChangeMainOutbound(outbound);
        
        if (mode == VpnMode.Proxy || mode == VpnMode.ProxyAndTun)
        {
            if (_vpnCore.IsSupportProtocol(VpnProtocol.Http))
            {
                var bound = new Http()
                {
                    Tag = "main-net-inbound-http",
                    Host = _settingsConfig.Network.Proxy.Http.Host,
                    Port = _settingsConfig.Network.Proxy.Http.Port,
                    Sniff = true,
                    SniffOverrideDestination = true
                };
                
                _vpnCore.AddInbound(bound);
            }

            if (_vpnCore.IsSupportProtocol(VpnProtocol.Socks))
            {
                var bound = new Socks()
                {
                    Tag = "main-net-inbound-socks",
                    Host = _settingsConfig.Network.Proxy.Socks.Host,
                    Port = _settingsConfig.Network.Proxy.Socks.Port,
                    Sniff = true,
                    SniffOverrideDestination = true
                };
                
                _vpnCore.AddInbound(bound);
            }
        }

        if (mode == VpnMode.Tun || mode == VpnMode.ProxyAndTun)
        {
            // NEEDS TO IMPLEMENT
            Console.WriteLine("SSTTART");
            if (_vpnCore.IsSupportProtocol(VpnProtocol.Tun))
            {
                Console.WriteLine("fwefwefwefwefwefewfewfwfwe");
                var bound = new Tun()
                {
                    Tag = "main-net-inbound-tun",
                    InterfaceName = "CrimsonShroud",
                    Address = new List<string>() { "172.18.0.1/30" },
                    Mtu = 9000,
                    AutoRoute = true,
                    StrictRoute = true,
                    Stack = "gvisor",
                    Sniff = true
                };
                
                _vpnCore.AddInbound(bound);
            }
            else
            {
                
            }
        }
        
        _vpnCore.ApplyConfiguration(_settingsConfig);
        _vpnCore.FixDnsIssues(credentials.TransparentHosts);
        
        if (_settingsConfig.SplitTunneling.Enabled == true)
            _vpnCore.ApplySplitTunneling(_settingsConfig.SplitTunneling);

        CurrentMode = mode;
        await _vpnCore.EnableAsync();
    }

    public async Task RestartAsync(VpnMode mode)
    {
        await DisableAsync();
        await EnableAsync(mode);

        CurrentMode = VpnMode.Disabled;
    }


    public async Task DisableAsync()
    {
        await _vpnCore.DisableAsync();
    }

    private void OnVpnEnabled(object? sender, EventArgs e)
    {
        string proxyAddress;
        if (CurrentMode == VpnMode.Proxy)
        {
            if (_settingsConfig.Network.Proxy.Default == VpnProtocol.Socks)
            {
                proxyAddress = $"socks={_settingsConfig.Network.Proxy.Socks.Host}:{_settingsConfig.Network.Proxy.Socks.Port}";
            }
            else
            {
                proxyAddress = $"{_settingsConfig.Network.Proxy.Http.Host}:{_settingsConfig.Network.Proxy.Http.Port}";
            }
        
            _proxyManager.EnableAsync(proxyAddress, _settingsConfig.Network.Proxy.ExcludedHosts).GetAwaiter().GetResult();
        }
        VpnEnabled?.Invoke(sender, e);
    }

    private void OnVpnDisabled(object? sender, EventArgs e)
    {
        _proxyManager.DisableAsync("", new List<string>());
        VpnDisabled?.Invoke(sender, e);
    }
}