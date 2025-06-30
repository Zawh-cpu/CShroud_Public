using System.Text.Json.Serialization;
using CShroudApp.Core.Configs;

namespace CShroudApp.Core.JsonContexts;

[JsonSourceGenerationOptions(WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.Never)]
[JsonSerializable(typeof(ApplicationConfig))]
[JsonSerializable(typeof(Dictionary<string, string>))]
public partial class ConfigsJsonContext : JsonSerializerContext;