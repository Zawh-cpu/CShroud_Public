using System.Text.Json;
using System.Text.Json.Serialization;
using CShroudDAW.Infrastructure.Cores.Xray.Services;
using Xray.Common.Serial;
using Xray.Proxy.Vless;

namespace CShroudDAW.Infrastructure.Cores.Xray.Mappers;

[JsonSerializable(typeof(VlessMapper.VlessCreds))]
public partial class MyJsonContext : JsonSerializerContext
{
}

public static class VlessMapper
{
    public class VlessCreds
    {
        public required string Id { get; set; }
        public required string Flow { get; set; }
        public string Encryption { get; set; } = "none";
    }
    
    public static TypedMessage? GetUser(string credentials)
    {
        var data = JsonSerializer.Deserialize<VlessCreds>(credentials, MyJsonContext.Default.VlessCreds);
        if (data == null) return null;
        return VpnRepository.ToTypedMessage( new Account()
        {
            Id = data.Id,
            Flow = data.Flow,
            Encryption = data.Encryption
        });
    }
}