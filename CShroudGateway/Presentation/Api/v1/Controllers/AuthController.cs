using CShroudGateway.Core.Interfaces;
using CShroudGateway.Presentation.Api.v1.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public partial class AuthController : ControllerBase
{
    private readonly ILogger<PingController> _logger;
    private readonly IBaseRepository _baseRepository;
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IQuickAuthService _quickAuth;

    public AuthController(ILogger<PingController> logger, IBaseRepository baseRepository, IAuthService authService, ITokenService tokenService, IUserService userService, IQuickAuthService quickAuth)
    {
        _logger = logger;
        _baseRepository = baseRepository;
        _authService = authService;
        _tokenService = tokenService;
        _userService = userService;
        _quickAuth = quickAuth;
    }
}