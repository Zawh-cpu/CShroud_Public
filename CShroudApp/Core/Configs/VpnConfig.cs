using System.Text.Json.Serialization;
using CShroudApp.Core.Entities;

namespace CShroudApp.Core.Configs;

public class VpnConfig
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VpnMode Mode { get; set; } = VpnMode.Proxy;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VpnCore Core { get; set; } = VpnCore.SingBox;

    public string PreferredProxy { get; set; } = "8.8.8.8";

    public InputsObject Inputs { get; set; } = new();


    public class InputsObject
    {
        public struct InputObj
        {
            public string Host { get; set; }
            public uint Port { get; set; }
        }
        
        public InputObj Http { get; set; } = new() { Host = "127.0.0.1", Port = 10808 };
        public InputObj Socks { get; set; } = new() { Host = "127.0.0.1", Port = 10809 };
    }
}
