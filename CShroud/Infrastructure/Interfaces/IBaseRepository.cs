using System.Linq.Expressions;
using CShroud.Infrastructure.Data;
using CShroud.Infrastructure.Data.Entities;

namespace CShroud.Infrastructure.Interfaces;

public interface IBaseRepository
{
    /*
    // Vitas DeMarsh
    
    
    public ApplicationContext GetContext();
    */
    string Ping();
    Task ExplicitLoadAsync<T>(ApplicationContext context, T entity, params Expression<Func<T, object>>[] navigationProperties) where T : class;
    Task<Key?> GetKeyAsync(ApplicationContext context, uint id);
    Task<Protocol?> GetProtocolAsync(ApplicationContext context, string id);
    Task<User?> GetUserAsync(ApplicationContext context, uint id, params Expression<Func<User, object>>[] includes);
    Task<bool> UserExistsAsync(ApplicationContext context, ulong telegramId);
    Task<List<User>> GetUsersPayedUntilAsync(ApplicationContext context, DateTime date);
    Task<List<User>> GetAllActiveKeysByUserAsync(ApplicationContext context);
    Task<int> CountKeysAsync(ApplicationContext context, uint userId, bool active = true);
}