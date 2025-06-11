using System.Text.Json.Serialization;
using CShroudApp.Application.DTOs;
using CShroudApp.Infrastructure.Data.Config;

namespace CShroudApp.Application.Serialization;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, WriteIndented = true)]
[JsonSerializable(typeof(FastLoginDto))]
[JsonSerializable(typeof(SignInDto))]
[JsonSerializable(typeof(QuickAuthSessionDto))]
[JsonSerializable(typeof(ActionRefreshDto))]
[JsonSerializable(typeof(GetUserDto))]
[JsonSerializable(typeof(SettingsConfig))]
public partial class AppJsonContext : JsonSerializerContext
{
}