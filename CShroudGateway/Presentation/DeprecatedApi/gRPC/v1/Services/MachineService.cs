using CShroudGateway.Core.Interfaces;
using CShroudGateway.Presentation.DeprecatedApi.gRPC.v1.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace CShroudGateway.Presentation.DeprecatedApi.gRPC.v1.Services;

public class MachineService : Machine.MachineBase
{
    private readonly ILogger<MachineService> _logger;
    private readonly IVpnRepository _vpnRepository;
    
    public MachineService(ILogger<MachineService> logger, IVpnRepository vpnRepository)
    {
        _logger = logger;
        _vpnRepository = vpnRepository;
    }

    public override Task<PingAnswer> Ping(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new PingAnswer() { Status = 1});
    }
}