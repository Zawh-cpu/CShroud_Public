using CShroudGateway.Core.Interfaces;
using CShroudGateway.Presentation.Api.ActionFilters;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[LocalhostOnlyFilter]
public partial class TelegramController : ControllerBase
{
    private readonly IBaseRepository _baseRepository;
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;

    public TelegramController(IBaseRepository baseRepository, IAuthService authService, ITokenService tokenService)
    {
        _baseRepository = baseRepository;
        _authService = authService;
        _tokenService = tokenService;
    }
}