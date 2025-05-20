using CShroudGateway.Application.Mappers.Protocols;
using System.Text.Json;
using CShroudGateway.Application.DTOs.Connection;
using CShroudGateway.Core.Entities;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Application.Factories;

public static class ProtocolConnectionFactory
{
    private static readonly Dictionary<VpnProtocol, IProtocolMapper> Mappers = new()
    {
        { VpnProtocol.Vless, new VlessMapper() }
    };

    public static IConnectionInfo CreateCredentials(ProtocolSettings credentials, Key key, Server server)
    {
        
        if (Mappers.TryGetValue(credentials.Protocol, out var mapper))
        {
            return mapper.GetCredentials(credentials, key, server);
        }
        
        throw new InvalidOperationException("Unknown protocol type");
    }
}