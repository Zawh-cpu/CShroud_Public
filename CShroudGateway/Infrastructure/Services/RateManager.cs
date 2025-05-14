using Ardalis.Result;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Infrastructure.Services;

public class RateManager : IRateManager
{
    private readonly IVpnKeyService _keyService;

    public RateManager(IVpnKeyService keyService)
    {
        _keyService = keyService;
    }
    
    public async Task<Result> ChangeRateAsync(User user, bool saveChanges = true)
    {
        if (user.Rate is null || !user.Keys.Any()) return Result.Success();

        var activeKeys = user.Keys.Count(k => k.Status == KeyStatus.Enabled);
        if (activeKeys >= user.Rate.MaxKeys)
        {
            var action = user.Keys.Where(k => k.Status == KeyStatus.Enabled).Skip(user.Rate.MaxKeys).ToArray();
            foreach (var key in action)
            {
                await _keyService.DisableKeyAsync(key, saveChanges: saveChanges);
                key.Status = KeyStatus.Revoked;
            }
        }
        else
        {
            var action = user.Keys.Where(k => k.Status == KeyStatus.Revoked).Take(user.Rate.MaxKeys - activeKeys).ToArray();
            foreach (var key in action) await _keyService.ForceEnableKeyAsync(key, user.Rate.VpnLevel, saveChanges: saveChanges);
        }
        
        return Result.Success();
    }
}