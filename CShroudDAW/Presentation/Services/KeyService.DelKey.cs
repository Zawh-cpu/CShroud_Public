using CShroudDAW.Core.Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Enum = System.Enum;

namespace CShroudDAW.Presentation.Services;

public partial class KeyService
{
    public override async Task<Empty> DelKey(DelKeyRequest request, ServerCallContext context)
    {
        if (!Enum.TryParse<VpnProtocol>(request.Protocol, out VpnProtocol vpnProtocol))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid protocol"));
        
        await _vpnKeyService.RemoveKey(vpnProtocol, request.Id);
        return new Empty();
    }
}