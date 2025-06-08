using Ardalis.Result;
using CShroudGateway.Core.Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Enum = System.Enum;

namespace CShroudGateway.Presentation.Api.v1.Services;

public partial class SyncService
{
    public override async Task<SyncKeyResponse> SyncKeys(SyncKeyRequest request, ServerCallContext context)
    {
        var server = await _baseRepository.GetServerByExpressionAsync(s => s.SecretKey == request.SecretKey);
        if (server is null) throw new RpcException(new Status(StatusCode.Unauthenticated, ""));

        var allKeysBelonged = await _baseRepository.GetKeysWithVpnLevelByExpressionAsync(k => k.ServerId == server.Id);
        
        var response = new SyncKeyResponse();
        response.Keys.AddRange(allKeysBelonged.Select(k => _vpnKeyService.GetKeyStruct(k.Key, k.VpnLevel))
            .Where(k => k is not null).ToArray());
        
        return response;
    }
}