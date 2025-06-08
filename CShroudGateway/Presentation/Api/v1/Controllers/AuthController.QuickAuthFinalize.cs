using CShroudGateway.Presentation.Api.v1.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class AuthController
{
    [HttpPost("quick-auth/{qaId:guid}/finalize")]
    public async Task<IActionResult> QuickAuthFinalize(Guid qaId)
    {
        using var reader = new StreamReader(Request.Body);
        var secretLoginCode = await reader.ReadToEndAsync();
        if (secretLoginCode == string.Empty) return new BadRequestResult();
        
        var session = _quickAuth.FinalizeSession(qaId.ToString());
        if (session is null || session?.SecretLoginCode != secretLoginCode) return NotFound();
        
        var user = await _baseRepository.GetUserByExpressionAsync(x => x.Id == session.ConfirmedUserId);
        if (user is null) return new UnauthorizedResult();

        var authDto = await _authService.SignInAsync(user, Request.Headers["User-Agent"].ToString(),
            HttpContext.Connection.RemoteIpAddress?.ToString());
        if (!authDto.IsSuccess) return new UnauthorizedResult();
        
        Console.WriteLine("OnQuickAuthFinalizedddddd");
        return new OkObjectResult(new SignInResponse
        {
            UserId = authDto.Value.UserId,
            ActionToken = authDto.Value.ActionToken,
            RefreshToken = authDto.Value.RefreshToken
        });
    }
}