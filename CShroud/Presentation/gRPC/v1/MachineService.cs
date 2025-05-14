using CShroud.Infrastructure.Interfaces;
using CShroud.Presentation.Protos.Server;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;

namespace CShroud.Presentation.gRPC.v1;

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
    
    public override async Task<Xray.App.Stats.Command.SysStatsResponse> GetSysStat(Empty request, ServerCallContext context)
    // public override Task<Xray.App.Stats.Command.SysStatsResponse> GetSysStat(Empty request, ServerCallContext context)
    {
        // throw new RpcException(new Status(StatusCode.Unimplemented, "offline"));
        
        
        var resp = await _vpnRepository.GetSysStat();
        if (resp == null)
        {
            throw new RpcException(new Status(StatusCode.Unavailable, "Service is unavailable"));
        }

        return resp;
        
    }
}