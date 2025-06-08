using Ardalis.Result;
using CShroudDAW;
using CShroudGateway.Core.Entities;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;
using Grpc.Core;

namespace CShroudGateway.Infrastructure.Services;

public class VpnRepository : IVpnRepository
{
    private readonly IGrpcPool _grpcPool;
    
    public VpnRepository(IGrpcPool grpcPool)
    {
        _grpcPool = grpcPool;
    }
    
    private async Task<Result<TResponse>> MakeRequest<TRequest, TResponse>(Func<TRequest, CallOptions, AsyncUnaryCall<TResponse>> grpcMethod, TRequest request)
    {
        try
        {
            Console.WriteLine("\n\n\nFAWEWEFWEF\n\n\n");
            var call = grpcMethod(request, new CallOptions());
            var response = await call.ResponseAsync;
            var status = call.GetStatus();

            Console.WriteLine($"Status: {status.StatusCode}");
            if (status.StatusCode != StatusCode.OK)
            {
                return Result.Error();
            }

            return response;
        }
        catch (RpcException)
        {
        }

        return Result.Error();
    }
    
    public async Task<Result<object>> AddKey(Server server, Guid keyId, VpnProtocol protocol, uint vpnLevel, string options)
    {
        var channel = _grpcPool.Get("http://" + server.IpV4Address);

        var client = new KeyService.KeyServiceClient(channel);

        var keyCommand = new AddKeyRequest()
        {
            Id = keyId.ToString(),
            VpnLevel = vpnLevel,
            Protocol = protocol.ToString(),
            Options = options,
        };
        
        var result = await MakeRequest(client.AddKeyAsync, keyCommand);
        return result.Map();
    }

    public async Task DelKey(Server server, Guid keyId, VpnProtocol protocol)
    {
        var channel = _grpcPool.Get("http://" + server.IpV4Address);

        var client = new KeyService.KeyServiceClient(channel);

        var keyCommand = new DelKeyRequest()
        {
            Id = keyId.ToString(),
            Protocol = protocol.ToString(),
        };
        
        _ = await MakeRequest(client.DelKeyAsync, keyCommand);
    }
}