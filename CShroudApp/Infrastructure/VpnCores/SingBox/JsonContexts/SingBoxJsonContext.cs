using System.Text.Json.Serialization;
using CShroudApp.Infrastructure.VpnCores.SingBox.Config;
using CShroudApp.Infrastructure.VpnCores.SingBox.Config.Bounds;
using CShroudApp.Infrastructure.VpnCores.SingBox.Mappers;

namespace CShroudApp.Infrastructure.VpnCores.SingBox.JsonContexts;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower, WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(TopConfig))]
[JsonSerializable(typeof(VlessCredentials))]
public partial class SingBoxJsonContext : JsonSerializerContext;