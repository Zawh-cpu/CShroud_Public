using System.Text.Json.Serialization;

namespace CShroudApp.Infrastructure.VpnCores.SingBox.Config.Bounds;

public class BoundObject
{
    public string Type { get; set; }
    public string Tag { get; set; }

    [JsonExtensionData] public object ExternalData { get; set; }
}