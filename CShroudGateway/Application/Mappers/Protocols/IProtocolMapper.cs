using System.Text.Json;
using CShroudGateway.Application.DTOs.Connection;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Application.Mappers.Protocols;

public interface IProtocolMapper
{
    public IConnectionInfo GetCredentials(ProtocolSettings settings, Key key, Server server);
}