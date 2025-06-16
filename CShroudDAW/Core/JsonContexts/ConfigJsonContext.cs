using System.Text.Json.Serialization;
using CShroudDAW.Core.Configs;

namespace CShroudDAW.Core.JsonContexts;

[JsonSourceGenerationOptions(WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.Never)]
[JsonSerializable(typeof(ApplicationConfig))]
public partial class ConfigsJsonContext : JsonSerializerContext;