using CShroudApp.Core.Entities;

namespace CShroudApp.Core.Configs;

public class VpnConfig
{
    public VpnMode Mode { get; set; } = VpnMode.Proxy;
    public VpnCore Core { get; set; } = VpnCore.SingBox;

    public string PreferredProxy { get; set; } = "8.8.8.8";

    public InputsObject Inputs { get; set; } = new();


    public class InputsObject
    {
        public struct Input
        {
            public string Host;
            public uint Port;
        }
        
        public Input Http { get; set; } = new() { Host = "127.0.0.1", Port = 10808 };
        public Input Socks { get; set; } = new() { Host = "127.0.0.1", Port = 10809 };
    }
}
