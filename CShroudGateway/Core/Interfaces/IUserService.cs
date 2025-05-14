using System.Linq.Expressions;
using System.Security.Claims;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Core.Interfaces;

public interface IUserService
{
    Task<User?> GetUserFromContext(ClaimsPrincipal principal, params Func<IQueryable<User>, IQueryable<User>>[] queryModifiers);
}