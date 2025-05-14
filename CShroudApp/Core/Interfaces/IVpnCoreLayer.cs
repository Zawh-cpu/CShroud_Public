using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Infrastructure.Data.Config;

namespace CShroudApp.Core.Interfaces;

public interface IVpnCoreLayer
{
    public bool IsProtocolSupported(VpnProtocol protocol);
    
    public void AddInbound(IVpnBound bound, int index = int.MaxValue);
    public void AddOutbound(IVpnBound bound, int index = int.MaxValue);
    public void RemoveInbound(string tag, bool startsWithMode = false);
    public void RemoveOutbound(string tag, bool startsWithMode = false);

    public Task StartProcessAsync();
    public Task KillProcessAsync();

    public void ConcatConfigs(SettingsConfig settings);
    
    public List<VpnProtocol> SupportedProtocols { get; }

    public void FixDnsIssues(List<string> transparentHosts);
    void ApplySplitTunneling(SettingsConfig.SplitTunnelingObject splitTunnelingObject);
    
    public event EventHandler? ProcessStarted;
    public event EventHandler? ProcessExited;
    
    public bool IsRunning { get; }
}