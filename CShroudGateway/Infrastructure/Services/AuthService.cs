using Ardalis.Result;
using CShroudGateway.Application.DTOs.Auth;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IBaseRepository _baseRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IBaseRepository baseRepository, ITokenService tokenService)
    {
        _baseRepository = baseRepository;
        _tokenService = tokenService;
    }
    
    public async Task<Result<AuthResultDto>> SignInAsync(User user, string? userAgent, string? ipAddress)
    {
        var refreshToken = _tokenService.GenerateJwtToken(user.Id, TokenType.Refresh);
        var actionToken = _tokenService.GenerateJwtToken(user.Id, TokenType.Action);
        
        user.LastLogin = refreshToken.CreatedAt;
        var loginLog = new LoginHistory()
        {
            UserId = user.Id,
            TokenId = refreshToken.Id,
            Ipv4Address = ipAddress,
            LoginTimeStamp = refreshToken.CreatedAt,
            DeviceInfo = userAgent
        };

        await _baseRepository.AddRangeAsync([refreshToken, actionToken], saveChanges: false);
        await _baseRepository.AddEntityAsync(loginLog, saveChanges: true);
        // All data has been saved.

        return new AuthResultDto()
        {
            UserId = user.Id,
            ActionToken = actionToken.TokenData,
            RefreshToken = refreshToken.TokenData
        };
    }
}