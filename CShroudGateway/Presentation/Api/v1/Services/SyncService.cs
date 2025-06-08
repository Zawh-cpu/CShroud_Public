using CShroudGateway.Core.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace CShroudGateway.Presentation.Api.v1.Services;

public partial class SyncService : CShroudGateway.SyncService.SyncServiceBase
{
    private readonly ILogger<SyncService> _logger;
    private readonly IVpnKeyService _vpnKeyService;
    private readonly IBaseRepository _baseRepository;
    
    public SyncService(ILogger<SyncService> logger, IVpnKeyService vpnKeyService, IBaseRepository baseRepository)
    {
        _logger = logger;
        _vpnKeyService = vpnKeyService;
        _baseRepository = baseRepository;
    }
}