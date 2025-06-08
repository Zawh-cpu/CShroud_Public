using System.Text.Json.Serialization;
using CShroudApp.Application.DTOs;

namespace CShroudApp.Application.Serialization;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(FastLoginDto))]
[JsonSerializable(typeof(SignInDto))]
[JsonSerializable(typeof(QuickAuthSessionDto))]
[JsonSerializable(typeof(ActionRefreshDto))]
[JsonSerializable(typeof(GetUserDto))]
public partial class AppJsonContext : JsonSerializerContext
{
}