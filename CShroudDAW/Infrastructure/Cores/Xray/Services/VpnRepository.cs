using Ardalis.Result;
using CShroudDAW.Core.Entities;
using CShroudDAW.Core.Interfaces;
using CShroudDAW.Infrastructure.Cores.Xray.Mappers;
using CShroudDAW.Infrastructure.Data.Config;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using Xray.App.Proxyman.Command;
using Xray.Common.Protocol;
using Xray.Common.Serial;

namespace CShroudDAW.Infrastructure.Cores.Xray.Services;

public class VpnRepository : IVpnRepository
{
    private readonly Dictionary<VpnProtocol, Func<string, TypedMessage?>> _protocolMappers = new()
    {
        [VpnProtocol.Vless] = VlessMapper.GetUser
    };
    private readonly GrpcChannel _channel;
    
    public VpnRepository(IOptions<ApplicationConfig> config)
    {
        _channel = GrpcChannel.ForAddress(config.Value.Vpn.Cores.Xray.ApiAddress);
    }
    
    public static TypedMessage ToTypedMessage(IMessage message)
    {
        return new TypedMessage
        {
            Type = message.Descriptor.FullName,
            Value = message.ToByteString()
        };
    }
    
    private async Task<Result<TResponse?>> MakeRequest<TRequest, TResponse>(Func<TRequest, CallOptions, AsyncUnaryCall<TResponse>> grpcMethod, TRequest request)
    {
        try
        {
            var call = grpcMethod(request, new CallOptions());
            var response = await call.ResponseAsync;
            var status = call.GetStatus();

            if (status.StatusCode != StatusCode.OK)
            {
                return Result.Invalid();
            }

            return response;
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"gRPC RpcException: {ex.Status.StatusCode} - {ex.Status.Detail}");
        }

        return Result.Unavailable();
    }
    
    public async Task<Result> AddKey(VpnProtocol protocol, uint vpnLevel, string email, string credentials)
    {
        var client = new HandlerService.HandlerServiceClient(_channel);
        
        if (!_protocolMappers.TryGetValue(protocol, out var mapper)) return Result.Unavailable();

        var account = mapper(credentials);
        if (account == null) return Result.Invalid();
        
        var request = new AlterInboundRequest()
        {
            Tag = $"inbound-{protocol}",
            Operation = ToTypedMessage(new AddUserOperation() { User = new User()
            {
                Level = vpnLevel,
                Email = email,
                Account = account
            } }),
        };
        
        Console.WriteLine(account);
        Console.WriteLine(email);
        Console.WriteLine(request.ToString());
        
        return (await MakeRequest(client.AlterInboundAsync, request)).IsSuccess ? Result.Success() : Result.Unavailable();
    }

    public async Task DelKey(VpnProtocol protocol, string connectionId)
    {
        var client = new HandlerService.HandlerServiceClient(_channel);

        var request = new AlterInboundRequest()
        {
            Tag = $"inbound-{protocol}",
            Operation = ToTypedMessage(new RemoveUserOperation()
            {
                Email = connectionId
            })
        };
        
        await MakeRequest(client.AlterInboundAsync, request);
    }
}