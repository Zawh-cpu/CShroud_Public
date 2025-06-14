using System.Text.Json.Serialization;
using CShroudApp.Core.Entities;

namespace CShroudApp.Core.Configs;

public class ApplicationConfig
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LogLevelMode LogLevel = LogLevelMode.Off;
    public NetworkConfig Network = new();
    
    public VpnConfig Vpn = new();
}
