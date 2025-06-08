using Ardalis.Result;
using CShroudDAW.Application.DTOs;
using CShroudDAW.Core.Entities;
using CShroudDAW.Core.Interfaces;
using CShroudGateway;
using Grpc.Core;

namespace CShroudDAW.Infrastructure.Services;

public class SyncService : ISyncService
{
    private readonly IGrpcPool _grpcPool;
    private readonly IVpnKeyService _vpnKeyService;
    
    public SyncService(IGrpcPool grpcPool, IVpnKeyService vpnKeyService)
    {
        _grpcPool = grpcPool;
        _vpnKeyService = vpnKeyService;
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
    
    public async Task<Result<SyncResponseDto>> SyncKeys(string gatewayAddress, string secretKey)
    {
        var channel = _grpcPool.Get(gatewayAddress);

        var client = new CShroudGateway.SyncService.SyncServiceClient(channel);

        var request = new SyncKeyRequest()
        {
            SecretKey = secretKey
        };
        
        var result = await MakeRequest(client.SyncKeysAsync, request);
        if (!result.IsSuccess) return result.Map();

        uint synced = 0;
        foreach (var keyStruct in result.Value.Keys)
        {
            if (!Enum.TryParse<VpnProtocol>(keyStruct.Protocol, out VpnProtocol vpnProtocol))
                continue;
            
            if ((await _vpnKeyService.AddKey(vpnProtocol, keyStruct.VpnLevel, keyStruct.Id, keyStruct.Options)).IsSuccess)
                synced++;
            
        }
        
        return new SyncResponseDto((uint)result.Value.Keys.Count, synced);
    }
}