using CShroud.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace CShroud.Presentation.gRPC.v1;

using Grpc.Core;
using CShroud.Presentation.Protos.Server;

public class UpdateService : CShroud.Presentation.Protos.Server.Update.UpdateBase
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
        if (request.CurrentHash == _updatePrimitive.GlobalParamsHash)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Your version is up to date already!"));
        }
        
        return Task.FromResult(_updatePrimitive.ProtoGlobalParams);
    }
}
