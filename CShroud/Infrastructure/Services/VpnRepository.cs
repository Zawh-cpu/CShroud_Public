using CShroud.Core.Domain.Entities;
using CShroud.Core.Domain.Interfaces;
using CShroud.Infrastructure.Data.Entities;
using CShroud.Infrastructure.Interfaces;
using Grpc.Core;
using Grpc.Net.Client;
using Xray.App.Proxyman.Command;
using Xray.App.Stats.Command;

namespace CShroud.Infrastructure.Services;

public class VpnRepository : IVpnRepository
{
    private GrpcChannel _channel;
    private readonly IVpnCore _vpnCore;
    private readonly VpnCoreConfig _vpnCoreConfig;
    private readonly IProtocolHandlerFactory _protocolHandlerFactory;
    
    public VpnRepository(IVpnCore vpnCore, VpnCoreConfig vpnCoreConfig, IProtocolHandlerFactory protocolHandlerFactory)
    {
        _vpnCore = vpnCore;
        _vpnCoreConfig = vpnCoreConfig;
        _protocolHandlerFactory = protocolHandlerFactory;
        
        _channel = GrpcChannel.ForAddress(_vpnCoreConfig.Link);
    }
    
    private async Task<TResponse?> MakeRequest<TRequest, TResponse>(Func<TRequest, CallOptions, AsyncUnaryCall<TResponse>> grpcMethod, TRequest request)
    {
        try
        {
            var call = grpcMethod(request, new CallOptions());
            var response = await call.ResponseAsync;
            var status = call.GetStatus();

            if (status.StatusCode != Grpc.Core.StatusCode.OK)
            {
                Console.WriteLine($"gRPC error: {status.StatusCode} - {status.Detail}");
                return default;
            }

            return response;
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"gRPC RpcException: {ex.Status.StatusCode} - {ex.Status.Detail}");
        }

        return default;
    }
    
    public async Task<bool> AddKey(uint vpnLevel, string uuid, string protocolId)
    {
        var client = new HandlerService.HandlerServiceClient(_channel);
        if (!_protocolHandlerFactory.Analyze(protocolId, out var protocolFactory)) return false;
        
        var userCmd = new AddUserOperation()
        {
            User = new Xray.Common.Protocol.User()
            {
                Level = vpnLevel,
                Email = uuid,
                Account = protocolFactory!.Invoke().MakeAccount(uuid, new System.Collections.Generic.Dictionary<string, string>())
            }
        };
        
        var request = new AlterInboundRequest()
        {
            Tag = $"inbound-{protocolId}",
            Operation = IVpnRepository.ToTypedMessage(userCmd)
        };

        var result = await MakeRequest(client.AlterInboundAsync, request);
        if (result != null) return true;
        return false;
    }

    public async Task<bool> DelKey(string uuid, string protocolId)
    {
        var client = new HandlerService.HandlerServiceClient(_channel);

        var request = new AlterInboundRequest()
        {
            Tag = $"inbound-{protocolId}",
            Operation = IVpnRepository.ToTypedMessage(new RemoveUserOperation()
            {
                Email = uuid
            })
        };
        
        var result = await MakeRequest(client.AlterInboundAsync, request);
        if (result != null) return true;
        return false;
    }

    public async Task<SysStatsResponse?> GetSysStat()
    {
        var client = new Xray.App.Stats.Command.StatsService.StatsServiceClient(_channel);
        return await MakeRequest(client.GetSysStatsAsync, new Xray.App.Stats.Command.SysStatsRequest());
    }
}