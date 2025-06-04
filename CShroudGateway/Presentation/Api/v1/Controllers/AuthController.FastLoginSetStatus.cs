using System.Security.Claims;
using CShroudGateway.Core.Constants;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Presentation.Api.v1.DataAnnotations;
using CShroudGateway.Presentation.Api.v1.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class AuthController
{
    [HttpPatch("fast_login/{flId:guid}/verify/{variant:int}")]
    [Authorize]
    public async Task<IActionResult> FastLoginSetVerifiedAsync(Guid flId, int variant)
    {
        if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId)) return BadRequest();

        var session = await _baseRepository.GetFastLoginByExpressionAsync(fl => fl.Id == flId);
        if (session is null || session.Status != FastLoginStatus.Pending || session.CreatedAt.AddMinutes(15) < DateTime.UtcNow) return NotFound();
        
        if (variant < 0 || session.Variants[0] != (uint)variant)
        {
            session.Status = FastLoginStatus.Declined;
            await _baseRepository.SaveContextAsync();
            return BadRequest();
        }
        
        session.VerifiedUserId = userId;
        session.Status = FastLoginStatus.Verified;
        await _baseRepository.SaveContextAsync();
        
        return Ok();
    }
}