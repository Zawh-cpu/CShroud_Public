using Ardalis.Result;
using CShroudGateway.Core.Entities;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Core.Interfaces;

public interface IVpnKeyService
{
    StructKey? GetKeyStruct(Key key, uint vpnLevel);
    
    Task<Result> ForceAddKeyAsync(Key key, uint vpnLevel);
    Task<Result> AddKeyAsync(Key key, User user);
    
    Task<Result> DelKeyAsync(Key key);
    
    Task<Result> ForceEnableKeyAsync(Key key, uint vpnLevel, bool saveChanges = true);
    Task<Result> EnableKeyAsync(Key key, User user, bool saveChanges = true);
    
    Task<Result> DisableKeyAsync(Key key, bool saveChanges = true);
}