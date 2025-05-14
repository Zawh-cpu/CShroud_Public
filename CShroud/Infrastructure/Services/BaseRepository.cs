using System.Linq.Expressions;
using CShroud.Infrastructure.Interfaces;
using CShroud.Infrastructure.Data;
using CShroud.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CShroud.Infrastructure.Services;

public class BaseRepository : IBaseRepository
{
    public string Ping()
    {
        return "Pong";
    }
    
    public async Task<bool> UserExistsAsync(ApplicationContext context, ulong telegramId)
    {
        return await context.Users.AnyAsync(u => u.TelegramId == telegramId);
    }   
    
    private static string GetPropertyName<T>(Expression<Func<T, object>> propertyExpression)
    {
        if (propertyExpression.Body is MemberExpression member)
        {
            return member.Member.Name;
        }

        if (propertyExpression.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
        {
            return unaryMember.Member.Name;
        }

        throw new InvalidOperationException("Invalid navigation property expression.");
    }
    
    public async Task ExplicitLoadAsync<T>(ApplicationContext context,
        T entity,
        params Expression<Func<T, object>>[] navigationProperties
    ) where T : class
    {
        foreach (var navigationProperty in navigationProperties)
        {
            string propertyName = GetPropertyName(navigationProperty);

            var entry = context.Entry(entity);
            
            var isCollection = entry.Metadata.FindNavigation(propertyName)?.IsCollection ?? false;

            if (isCollection)
            {
                await entry.Collection(propertyName).LoadAsync();
            }
            else
            {
                await entry.Reference(propertyName).LoadAsync();
            }
        }
    }
    
    public async Task<User?> GetUserAsync(ApplicationContext context, uint id, params Expression<Func<User, object>>[] includes)
    {
        IQueryable<User> query = context.Users;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        
        return await query.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Protocol?> GetProtocolAsync(ApplicationContext context, string id)
    {
        return await context.Protocols.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Key?> GetKeyAsync(ApplicationContext context, uint id)
    {
        return await context.Keys.FirstOrDefaultAsync(k => k.Id == id);
    }

    public async Task<int> CountKeysAsync(ApplicationContext context, uint userId, bool active = true)
    {
        return await context.Keys.AsNoTracking().Where(k => k.UserId == userId && k.IsActive == active).CountAsync();
    }

    public async Task<List<User>> GetAllActiveKeysByUserAsync(ApplicationContext context)
    {
        return await context.Users.Where(u => u.RateId > 1).Include(u => u.Rate).Include(u => u.Keys.Where(k => k.IsActive == true)).ToListAsync();
    }
    
    public async Task<List<User>> GetUsersPayedUntilAsync(ApplicationContext context, DateTime date)
    {
        return await context.Users.Where(u => u.PayedUntil <= date).ToListAsync();
    }
}
