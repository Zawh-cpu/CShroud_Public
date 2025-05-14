using Ardalis.Result;
using CShroudGateway.Application.DTOs.Auth;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Core.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResultDto>> SignInAsync(User user, string? userAgent, string? ipAddress);
}