using System.Collections.Concurrent;
using CShroudGateway.Core.Interfaces;
using Grpc.Net.Client;

namespace CShroudGateway.Infrastructure.Services;

public class GrpcPool : IGrpcPool
{
    private readonly ConcurrentDictionary<string, GrpcChannel> _pool = new();
    private readonly SocketsHttpHandler _grpcHandler = new SocketsHttpHandler
    {
        PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
        KeepAlivePingDelay = TimeSpan.FromSeconds(30),
        KeepAlivePingTimeout = TimeSpan.FromSeconds(15),
        EnableMultipleHttp2Connections = true,
        MaxConnectionsPerServer = 100
    };
    public GrpcChannel Get(string address)
    {
        return _pool.GetOrAdd(address, _ => GrpcChannel.ForAddress(address, new GrpcChannelOptions
        {
            HttpHandler = _grpcHandler
        }));
    }
}