using CShroudDAW.Core.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace CShroudDAW.Presentation.Services;

public partial class KeyService : CShroudDAW.KeyService.KeyServiceBase
{
    private readonly ILogger<GreeterService> _logger;
    private readonly IVpnKeyService _vpnKeyService;
    
    public KeyService(ILogger<GreeterService> logger, IVpnKeyService vpnKeyService)
    {
        _logger = logger;
        _vpnKeyService = vpnKeyService;
    }
}