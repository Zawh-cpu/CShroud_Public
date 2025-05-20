using System.Text.Json;
using CShroudGateway.Application.DTOs.Connection;
using CShroudGateway.Core.Entities.Protocols;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Application.Mappers.Protocols;

public class VlessMapper : IProtocolMapper
{
    public IConnectionInfo GetCredentials(ProtocolSettings settings, Key key, Server server)
    {
        var vlessData = settings.ExtraContent.Deserialize<VlessSettings>();
        if (vlessData == null || settings.Server is null) throw new ArgumentException();

        return new VlessConnection()
        {
            Host = settings.Server.IpV4Address,
            Port = settings.Port,
            Uuid = key.Id,
            Flow = vlessData.Flow,
            ServerName = vlessData.ServerName,
            Insecure = vlessData.Insecure,
            PublicKey = vlessData.PublicKey,
            ShortId = vlessData.ShortId
        };
    }
    
    public static string MakeOptions(Guid keyId)
    {
        return $"{{\"Id\": \"{keyId}\", \"Flow\": \"xtls-rprx-vision\", \"Encryption\": \"none\"}}";
    }
}