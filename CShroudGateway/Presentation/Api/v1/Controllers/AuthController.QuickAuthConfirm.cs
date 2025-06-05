using CShroudGateway.Core.Entities;
using CShroudGateway.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class AuthController
{
    [HttpPost("quick-auth/{qaId:guid}/confirm")]
    [Authorize]
    public async Task<IActionResult> QuickAuthCreate(Guid qaId, [FromQuery] int variant)
    {
        var session = _quickAuth.GetSession(qaId.ToString());
        if (session is null) return NotFound();
        
        if (variant < 0 || session.ValidVariant != (uint)variant)
        {
            await _quickAuth.DeclineSession(session);
            return Ok(new { Status = "cancelled" });
        }

        if (session.ExpiresAt < DateTime.UtcNow || session.Status != QuickAuthStatus.Pending)
        {
            return Ok(new { Status = "expired" });
        }

        if (!User.TryGetUserId(out Guid userId))
            return Unauthorized(new { Status = "UserId error. Maybe token is broken." });

        await _quickAuth.ConfirmSession(session, userId);
        return Ok(new { Status = "confirmed" });
    }
}