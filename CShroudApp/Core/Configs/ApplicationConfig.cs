using CShroudApp.Core.Entities;

namespace CShroudApp.Core.Configs;

public class ApplicationConfig
{
    public LogLevelMode LogLevel = LogLevelMode.Off;
    public NetworkConfig Network = new();
    
    public VpnConfig Vpn = new();
}
