using System.Text.Json.Serialization;
using CShroudApp.Infrastructure.VpnCores.SingBox.Mappers;

namespace CShroudApp.Infrastructure.VpnCores.SingBox.JsonContexts;

[JsonSourceGenerationOptions(WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault)]
[JsonSerializable(typeof(VlessCredentials))]
public partial class CredentialsJsonContext : JsonSerializerContext;