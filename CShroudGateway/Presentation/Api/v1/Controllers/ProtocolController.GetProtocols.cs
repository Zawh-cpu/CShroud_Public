using System.Security.Claims;
using CShroudGateway.Core.Entities;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Presentation.Api.v1.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class ProtocolController
{
    [HttpGet("available")]
    public IActionResult GetProtocolsAsync()
    {
        return Ok(Enum.GetValues<VpnProtocol>().Select(x => x.ToString()));
    }
}