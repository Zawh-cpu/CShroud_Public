using System.Security.Claims;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Presentation.Api.v1.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class AuthController
{
    [HttpPost("quick-auth")]
    public IActionResult QuickAuthCreate()
    {
        var session = _quickAuth.CreateSession();
        return Ok(new { sessionId = session.SessionId, validVariant = session.ValidVariant, variants = session.Variants });
    }
}