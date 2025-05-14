using CShroud.Infrastructure.Data;
using CShroud.Infrastructure.Data.Entities;

namespace CShroud.Infrastructure.Interfaces;

public interface IRateManager
{
    Task UpdateRate(ApplicationContext context, User user);
}