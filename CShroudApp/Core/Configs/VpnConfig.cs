using CShroudApp.Core.Entities;

namespace CShroudApp.Core.Configs;

public class VpnConfig
{
    public VpnMode Mode { get; set; } = VpnMode.Proxy;
    public VpnCore Core { get; set; } = VpnCore.SingBox;
}
