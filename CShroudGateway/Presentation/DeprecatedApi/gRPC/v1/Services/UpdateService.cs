using CShroudGateway.Core.Interfaces;
using CShroudGateway.Presentation.DeprecatedApi.gRPC.v1.Protos;
using Grpc.Core;

namespace CShroudGateway.Presentation.DeprecatedApi.gRPC.v1.Services;

public class UpdateService : Update.UpdateBase
{
    private readonly ILogger<UpdateService> _logger;
    private readonly IUpdatePrimitive _updatePrimitive;

    public UpdateService(ILogger<UpdateService> logger, IUpdatePrimitive updatePrimitive)
    {
        _logger = logger;
        _updatePrimitive = updatePrimitive;
    }

    public override Task<UpdateBytes> GetGlobalParams(GlobalParamsRequest request,
        ServerCallContext context)
    {
        throw new RpcException(new Status(StatusCode.AlreadyExists, "Your version is up to date already!"));
        /*if (request.CurrentHash == _updatePrimitive.GlobalParamsHash)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Your version is up to date already!"));
        }
        
        return Task.FromResult(_updatePrimitive.ProtoGlobalParams);
        */
    }
}
