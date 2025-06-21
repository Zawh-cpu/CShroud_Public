using System.Text.Json.Serialization;

namespace CShroudApp.Infrastructure.VpnCores.SingBox.Config.Bounds;


[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(SocksBound), "socks")]
[JsonDerivedType(typeof(HttpBound), "http")]
[JsonDerivedType(typeof(VlessBound), "vless")]
[JsonDerivedType(typeof(BlockBound), "block")]
[JsonDerivedType(typeof(DirectBound), "direct")]
[JsonDerivedType(typeof(DnsBound), "dns")]
[JsonDerivedType(typeof(TunBound), "tun")]
public class BoundObject
{
    public string? Tag { get; set; }

    [JsonExtensionData] public Dictionary<string , object>? ExternalData { get; set; }
}