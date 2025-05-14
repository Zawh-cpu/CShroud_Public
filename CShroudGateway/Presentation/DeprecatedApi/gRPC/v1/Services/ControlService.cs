using Ardalis.Result;
using CShroudGateway.Core.Entities;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Presentation.DeprecatedApi.gRPC.v1.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Enum = System.Enum;

namespace CShroudGateway.Presentation.DeprecatedApi.gRPC.v1.Services;

public class ControlService : Control.ControlBase
{
    private readonly ILogger<ControlService> _logger;
    private readonly IBaseRepository _baseRepository;
    private readonly IVpnKeyService _vpnKeyService;
    private readonly IVpnService _vpnService;
    private readonly IVpnServerManager _vpnServerManager;

    public ControlService(ILogger<ControlService> logger, IBaseRepository baseRepository, IVpnKeyService vpnKeyService, IVpnService vpnService, IVpnServerManager vpnServerManager)
    {
        _logger = logger;
        _baseRepository = baseRepository;
        _vpnKeyService = vpnKeyService;
        _vpnService = vpnService;
        _vpnServerManager = vpnServerManager;
    }

    public override async Task<Empty> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        if (await _baseRepository.IsUserWithThisExpressionExistsAsync(u => u.TelegramId == Convert.ToInt64(request.TelegramId)))
            throw new RpcException(new Status(StatusCode.Cancelled, "User already exists"));

        var user = new User()
        {
            Nickname = request.Nickname.Substring(0, Math.Min(request.Nickname.Length, 96)),
            TelegramId = Convert.ToInt64(request.TelegramId)
        };

        await _baseRepository.AddWithSaveAsync(user);
        
        return new Empty();
    }
    
    public override async Task<AddClientResponse> AddClient(AddClientRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out Guid userId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "UserId is invalid"));

        if (!Enum.TryParse(request.ProtocolId, out VpnProtocol protocol))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid protocol specified"));

        var server = await _vpnServerManager.GetAvailableServerAsync("frankfurt", protocol);
        if (server is null) throw new RpcException(new Status(StatusCode.NotFound, "Server not found or its under maintenance"));

        var user = await _baseRepository.GetUserByIdAsync(userId, x => x.Include(u => u.Rate));
        if (user is null) throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

        var key = new Key()
        {
            Id = Guid.NewGuid(),
            ServerId = server.Id,
            Server = server,
            UserId = user.Id,
            User = user,
            Protocol = protocol,
            Name = request.Name
        };
        
        var result = await _vpnKeyService.AddKeyAsync(key, user);
        if (!result.IsSuccess)
        {
            if (result.IsUnavailable())
                throw new RpcException(new Status(StatusCode.Unavailable, "This DAW server is unavailable"));
            if (result.IsForbidden())
                throw new RpcException(new Status(StatusCode.PermissionDenied, "You've reached the maximum amount of keys"));
            throw new RpcException(new Status(StatusCode.Internal, "Internal error or invalid arguments. Please, try later"));
        }
        
        return new AddClientResponse()
        {   
            Id = key.Id.ToString()
        };
    }
    
    
    
    public override async Task<Empty> DelClient(RemClientRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out Guid userId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "UserId is invalid"));
        
        if (!Guid.TryParse(request.KeyId, out Guid keyId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "KeyId is invalid"));

        var key = await _baseRepository.GetKeyByIdAsync(keyId, x => x.Include(k => k.Server));
        if (key is null || key.UserId != userId) throw new RpcException(new Status(StatusCode.NotFound, "Key not found"));
        
        var result = await _vpnKeyService.DelKeyAsync(key);
        if (!result.IsSuccess)
            throw new RpcException(new Status(StatusCode.Internal, "Internal error, unauthorized access or invalid arguments. Please, try later"));
        
        return new Empty();
    }
    
    public override async Task<Empty> EnableKey(KeyRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out Guid userId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "UserId is invalid"));
        
        if (!Guid.TryParse(request.KeyId, out Guid keyId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "KeyId is invalid"));
        
        var key = await _baseRepository.GetKeyByIdAsync(keyId, x => x.Include(k => k.Server),
            x => x.Include(k => k.User).ThenInclude(u => u!.Rate));
        if (key is null || key.User?.Id != userId) throw new RpcException(new Status(StatusCode.NotFound, "Key not found"));
        
        var result = await _vpnKeyService.EnableKeyAsync(key, key.User);
        if (!result.IsSuccess)
            throw new RpcException(new Status(StatusCode.Internal, "Internal error, unauthorized access or invalid arguments. Please, try later"));
        
        return new Empty();
    }
    
    
    public override async Task<Empty> DisableKey(KeyRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out Guid userId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "UserId is invalid"));
        
        if (!Guid.TryParse(request.KeyId, out Guid keyId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "KeyId is invalid"));
        
        var key = await _baseRepository.GetKeyByIdAsync(keyId, x => x.Include(k => k.Server));
        if (key is null || key.UserId != userId) throw new RpcException(new Status(StatusCode.NotFound, "Key not found"));
        
        var result = await _vpnKeyService.DisableKeyAsync(key);
        if (!result.IsSuccess)
            throw new RpcException(new Status(StatusCode.Internal, "Internal error, unauthorized access or invalid arguments. Please, try later"));
        
        return new Empty();
    }
}
