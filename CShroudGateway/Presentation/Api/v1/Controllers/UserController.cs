using CShroudGateway.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public partial class UserController : ControllerBase
{
    private readonly IBaseRepository _baseRepository;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public UserController(IBaseRepository baseRepository, IUserService userService, ITokenService tokenService)
    {
        _baseRepository = baseRepository;
        _userService = userService;
        _tokenService = tokenService;
    }
}