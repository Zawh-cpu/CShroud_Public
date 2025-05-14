using CShroudGateway.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public partial class KeyController : ControllerBase
{
    private readonly ILogger<PingController> _logger;
    private readonly IBaseRepository _baseRepository;
    private readonly IUserService _userService;
    private readonly IVpnKeyService _keyService;
    private readonly IVpnServerManager _vpnServerManager;

    public KeyController(ILogger<PingController> logger, IBaseRepository baseRepository, IUserService userService, IVpnKeyService keyService, IVpnServerManager serverManager)
    {
        _logger = logger;
        _baseRepository = baseRepository;
        _userService = userService;
        _keyService = keyService;
        _vpnServerManager = serverManager;
    }
}