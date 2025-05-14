using CShroudGateway.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public partial class ProtocolController : ControllerBase
{
    private readonly ILogger<PingController> _logger;
    private readonly IBaseRepository _baseRepository;

    public ProtocolController(ILogger<PingController> logger, IBaseRepository baseRepository)
    {
        _logger = logger;
        _baseRepository = baseRepository;
    }
}