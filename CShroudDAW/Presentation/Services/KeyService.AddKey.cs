using System.Text.Json;
using Ardalis.Result;
using CShroudDAW.Core.Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Enum = System.Enum;

namespace CShroudDAW.Presentation.Services;

public partial class KeyService
{
    public override async Task<Empty> AddKey(AddKeyRequest request, ServerCallContext context)
    {
        if (!Enum.TryParse<VpnProtocol>(request.Protocol, out VpnProtocol vpnProtocol))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid protocol"));
        
        var result = await _vpnKeyService.AddKey(vpnProtocol, request.VpnLevel, request.Id, request.Options);
        if (!result.IsSuccess)
        {
            switch (result.Status)
            {
                case ResultStatus.Invalid:
                    throw new RpcException(new Status(StatusCode.InvalidArgument, ""));
                default:
                    throw new RpcException(new Status(StatusCode.Internal, ""));
            }
        }
        return new Empty();
    }
}