using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Infrastructure.Data.Config;

namespace CShroudApp.Core.Interfaces;

public interface IVpnCore
{
    Task EnableAsync();
    Task DisableAsync();
    
    void ClearMainInbounds();
    void AddInbound(IVpnBound bound);
    void ChangeMainOutbound(IVpnBound bound);
    
    bool IsSupportProtocol(VpnProtocol protocol);

    void FixDnsIssues(List<string> transparentHosts);
    
    bool IsRunning { get; }
    
    List<VpnProtocol> SupportedProtocols { get; }

    void ApplyConfiguration(SettingsConfig settings);
    void ApplySplitTunneling(SettingsConfig.SplitTunnelingObject splitTunnelingObject);
    
    event EventHandler? VpnEnabled;
    event EventHandler? VpnDisabled;
}