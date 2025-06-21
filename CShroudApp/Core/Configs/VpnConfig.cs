using System.Text.Json.Serialization;
using CShroudApp.Core.Entities;

namespace CShroudApp.Core.Configs;

public class VpnConfig
{
    [JsonConverter(typeof(JsonStringEnumConverter<VpnMode>))]
    public VpnMode Mode { get; set; } = VpnMode.Proxy;
    [JsonConverter(typeof(JsonStringEnumConverter<VpnCore>))]
    public VpnCore Core { get; set; } = VpnCore.SingBox;

    // If RM = false then pass through only via vpn and bypass that in SplitTunneling.
    // If RM = true then bypass all and only pass all that in SplitTunneling.
    public bool ReverseMode { get; set; } = false;
    public bool SavePreviousProxy { get; set; } = true;
    
    public string PreferredProxy { get; set; } = "8.8.8.8";

    public SplitTunnelingConfig SplitTunneling { get; set; } = new();
    

    public InputsObject Inputs { get; set; } = new();


    public class InputsObject
    {
        public struct InputObj
        {
            public string Host { get; set; }
            public uint Port { get; set; }
        }
        
        [JsonConverter(typeof(JsonStringEnumConverter<ProxyProtocol>))]
        public ProxyProtocol PreferredInput { get; set; } = ProxyProtocol.Socks;
        
        public string[] ExcludeProxyForAddresses { get; set; } = [];
        
        public InputObj Http { get; set; } = new() { Host = "127.0.0.1", Port = 10808 };
        public InputObj Socks { get; set; } = new() { Host = "127.0.0.1", Port = 10809 };
    }
}
