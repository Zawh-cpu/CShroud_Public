using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class PingController
{
    [HttpGet("")]
    public IActionResult ActionResultGet()
    {
        return Ok("Pong");
    }
}