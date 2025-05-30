using System.Text.Json.Serialization;
using CShroudApp.Application.DTOs;

namespace CShroudApp.Application.Serialization;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(FastLoginDto))]
public partial class AppJsonContext : JsonSerializerContext
{
}