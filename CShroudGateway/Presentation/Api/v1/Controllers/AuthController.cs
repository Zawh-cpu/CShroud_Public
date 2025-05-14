using CShroudGateway.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public partial class AuthController : ControllerBase
{
    private readonly ILogger<PingController> _logger;
    private readonly IBaseRepository _baseRepository;
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;

    public AuthController(ILogger<PingController> logger, IBaseRepository baseRepository, IAuthService authService, ITokenService tokenService)
    {
        _logger = logger;
        _baseRepository = baseRepository;
        _authService = authService;
        _tokenService = tokenService;
    }
}