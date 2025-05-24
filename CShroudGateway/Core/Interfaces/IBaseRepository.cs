using System.Linq.Expressions;
using CShroudGateway.Core.Entities;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Core.Interfaces;

public record UserWithKeys(User User, int KeysCount);
public record UserKeyActiveKeysCount(User User, int KeysCount, int ActiveKeysCount);

public interface IBaseRepository
{
    Task<bool> IsUserWithThisExpressionExistsAsync(Expression<Func<User, bool>> predicate);
    Task<int> CountKeysAsync(Guid userId, Expression<Func<Key, bool>>? predicate = null);
    Task<User?> GetUserByIdAsync(Guid userId, params Func<IQueryable<User>, IQueryable<User>>[] queryModifiers);
    Task<User?> GetUserByExpressionAsync(Expression<Func<User, bool>> predicate, params Func<IQueryable<User>, IQueryable<User>>[] queryModifiers);
    Task<Token?> GetTokenByIdAsync(Guid tokenId);
    Task AddWithSaveAsync<TEntity>(TEntity entity) where TEntity : class;
    Task AddEntityAsync<TEntity>(TEntity entity, bool saveChanges = true) where TEntity : class;
    Task DelWithSaveAsync<TEntity>(TEntity entity) where TEntity : class;

    Task<UserWithKeys?> GetUserByIdWithKeyCountAsync(Guid userId, params Func<IQueryable<User>, IQueryable<User>>[] queryModifiers);

    Task<Key?> GetKeyByIdAsync(Guid keyId, params Func<IQueryable<Key>, IQueryable<Key>>[] queryModifiers);

    Task<Rate?> GetFirstDefaultRateAsync();
    Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entity, bool saveChanges = true) where TEntity : class;
    
    Task<List<Server>?> GetServersByLocationAndProtocolsAsync(string location, HashSet<VpnProtocol> protocols, int limit = 3, params Func<IQueryable<Server>, IQueryable<Server>>[] queryModifiers);

    Task<UserKeyActiveKeysCount?> GetUserKeysActiveKeysCountByIdsAsync(Guid userId,
        params Func<IQueryable<User>, IQueryable<User>>[] queryModifiers);
    
    Task<User[]> GetUsersPayedUntilAsync(Expression<Func<User, bool>> predicate, params Func<IQueryable<User>, IQueryable<User>>[] queryModifiers);
    
    Task<Rate[]> GetRatesByExpressionAsync(Expression<Func<Rate, bool>> predicate);
    Task<Role[]> GetRolesByExpressionAsync(Expression<Func<Role, bool>> predicate);
    
    Task<Key[]> GetKeysByExpressionAsync(Expression<Func<Key, bool>> predicate, params Func<IQueryable<Key>, IQueryable<Key>>[] queryModifiers);

    Task<ProtocolSettings?> GetProtocolSettingsAsync(Expression<Func<ProtocolSettings, bool>> predicate,
        params Func<IQueryable<ProtocolSettings>, IQueryable<ProtocolSettings>>[] queryModifiers);

    Task<User[]> GetUsersByExpressionAsync(Expression<Func<User, bool>> predicate,
        params Func<IQueryable<User>, IQueryable<User>>[] queryModifiers);
    
    Task SaveContextAsync();
}