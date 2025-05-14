using System.Linq.Expressions;
using System.Security.Claims;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IBaseRepository _baseRepository;

    public UserService(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository;
    }
    
    public async Task<User?> GetUserFromContext(ClaimsPrincipal principal, params Func<IQueryable<User>, IQueryable<User>>[] queryModifiers)
    {
        // principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
        // principal.FindFirst("sub")?.Value;
        string? rawJti = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(rawJti, out Guid jti)) return null;
        
        return await _baseRepository.GetUserByIdAsync(jti, queryModifiers);
    }
}