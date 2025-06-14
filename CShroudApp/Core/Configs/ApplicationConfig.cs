using System.Text.Json.Serialization;
using CShroudApp.Core.Entities;

namespace CShroudApp.Core.Configs;

public class ApplicationConfig
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LogLevelMode LogLevel { get; set; } = LogLevelMode.Off;
    public NetworkConfig Network { get; set; } = new();
    
    public VpnConfig Vpn { get; set; } = new();
}
