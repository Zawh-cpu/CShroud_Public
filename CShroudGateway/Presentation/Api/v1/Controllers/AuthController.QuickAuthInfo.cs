using CShroudGateway.Core.Entities;
using CShroudGateway.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class AuthController
{
    [HttpGet("quick-auth/{qaId:guid}/info")]
    public IActionResult QuickAuthInfo(Guid qaId)
    {
        var session = _quickAuth.GetSession(qaId.ToString());
        if (session is null) return NotFound();
        
        return Ok(new { Id = qaId, Variants = session.Variants });
    }
}