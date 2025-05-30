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
    [HttpGet("fast_login/{flId:guid}/checkout")]
    public async Task<IActionResult> FastLoginCheckoutAsync(Guid flId, [FromBody] FastLoginCheckoutRequest request)
    {
        var session = await _baseRepository.GetFastLoginByExpressionAsync(fl => fl.Id == flId);
        if (session is null || session.Status != FastLoginStatus.Verified || session.Secret != request.Secret) return NotFound();


        var user = await _baseRepository.GetUserByIdAsync(session.VerifiedUserId);
        if (user is null)
        {
            session.Status = FastLoginStatus.Declined;
            await _baseRepository.SaveContextAsync();
            return OftenErrors.InvalidUser;
        }

        session.Status = FastLoginStatus.Used;
        var authDto = await _authService.SignInAsync(user, Request.Headers["User-Agent"].ToString(),
            HttpContext.Connection.RemoteIpAddress?.ToString());
        if (!authDto.IsSuccess)
        {
            session.Status = FastLoginStatus.Declined;
            await _baseRepository.SaveContextAsync();
            return new UnauthorizedResult();
        }
        
        return new OkObjectResult(new SignInResponse
        {
            UserId = authDto.Value.UserId,
            ActionToken = authDto.Value.ActionToken,
            RefreshToken = authDto.Value.RefreshToken
        });
    }
}