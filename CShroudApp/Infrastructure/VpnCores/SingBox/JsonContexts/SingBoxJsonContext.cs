using System.Text.Json.Serialization;
using CShroudApp.Infrastructure.VpnCores.SingBox.Config;
using CShroudApp.Infrastructure.VpnCores.SingBox.Config.Bounds;
using CShroudApp.Infrastructure.VpnCores.SingBox.Mappers;

namespace CShroudApp.Infrastructure.VpnCores.SingBox.JsonContexts;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower, WriteIndented = true)]
[JsonSerializable(typeof(TopConfig))]
[JsonSerializable(typeof(DnsObject.Rule))]
[JsonSerializable(typeof(RouteObject.Rule))]
[JsonSerializable(typeof(RouteObject.RuleSetObject))]
[JsonSerializable(typeof(List<RouteObject.Rule>))] // <-- вот это нужно!
[JsonSerializable(typeof(List<DnsObject.Rule>))]
[JsonSerializable(typeof(List<>))] // можно оставить, но оно универсально и в этом случае не поможет
[JsonSerializable(typeof(List<BoundObject>))]
[JsonSerializable(typeof(DnsObject))]
[JsonSerializable(typeof(DnsObject.Rule))]
[JsonSerializable(typeof(List<DnsObject.Rule>))]
[JsonSerializable(typeof(List<Dictionary<string, object>>))]
[JsonSerializable(typeof(Dictionary<string, object>))]
[JsonSerializable(typeof(VlessCredentials))]
public partial class SingBoxJsonContext : JsonSerializerContext;