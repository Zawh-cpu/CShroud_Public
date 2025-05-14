using Ardalis.Result;
using CShroudGateway.Application.Mappers.Protocols;
using CShroudGateway.Core.Entities;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Infrastructure.Services;

public class VpnKeyService : IVpnKeyService
{
    private readonly IBaseRepository _baseRepository;
    private readonly IVpnRepository _vpnRepository;
    
    private readonly Dictionary<VpnProtocol, Func<Guid, string>> _protocolMappers = new()
    {
        [VpnProtocol.Vless] = VlessMapper.MakeOptions
    };
    
    public VpnKeyService(IBaseRepository baseRepository, IVpnRepository vpnRepository)
    {
        _baseRepository = baseRepository;
        _vpnRepository = vpnRepository;
    }
    
    public async Task<Result> ForceAddKeyAsync(Key key, uint vpnLevel)
    {
        if (key.Server is null) throw new ArgumentNullException(nameof(key.Server));
        
        if (!_protocolMappers.TryGetValue(key.Protocol, out var mapper))
            return Result.Unavailable();

        var result = await _vpnRepository.AddKey(key.Server, key.Id, key.Protocol, vpnLevel, mapper(key.Id));
        if (!result.IsSuccess) return result.Map();
        await _baseRepository.AddWithSaveAsync(key);
        return Result.Success();
    }
    
    public async Task<Result> AddKeyAsync(Key key, User user)
    {
        if (user.Rate is null) throw new ArgumentNullException(nameof(user.Rate));

        // var activeKeys = await _baseRepository.CountKeysAsync(user.Id, x => x.Status == KeyStatus.Enabled);
        var activeKeys = await _baseRepository.CountKeysAsync(user.Id);
        if (activeKeys >= user.Rate.MaxKeys) return Result.Forbidden();

        return (await ForceAddKeyAsync(key, user.Rate.VpnLevel)).Map();
    }

    public async Task<Result> DelKeyAsync(Key key)
    {
        if (key.Server is not null)
            await _vpnRepository.DelKey(key.Server, key.Id, key.Protocol);
        
        await _baseRepository.DelWithSaveAsync(key);
        return Result.Success();
    }

    public async Task<Result> ForceEnableKeyAsync(Key key, uint vpnLevel, bool saveChanges = true)
    {
        if (key.Server is null) throw new ArgumentNullException(nameof(key.Server));
        
        if (!_protocolMappers.TryGetValue(key.Protocol, out var mapper))
            return Result.Unavailable();

        var result = await _vpnRepository.AddKey(key.Server, key.Id, key.Protocol, vpnLevel, mapper(key.Id));
        if (!result.IsSuccess) return Result.Error();
        
        key.Status = KeyStatus.Enabled;
        if (saveChanges) await _baseRepository.SaveContextAsync();
        
        return Result.Success();
    }

    public async Task<Result> EnableKeyAsync(Key key, User user, bool saveChanges = true)
    {
        if (user.Rate is null) throw new ArgumentNullException(nameof(user.Rate));
        switch (key.Status)
        {
            case KeyStatus.Revoked:
                return Result.Forbidden();
            case KeyStatus.Enabled:
                return Result.Success();
        }

        var activeKeys = await _baseRepository.CountKeysAsync(user.Id, x => x.Status == KeyStatus.Enabled);
        if (activeKeys >= user.Rate.MaxKeys) return Result.Forbidden();

        return (await ForceEnableKeyAsync(key, user.Rate.VpnLevel, saveChanges)).Map();
    }

    public async Task<Result> DisableKeyAsync(Key key, bool saveChanges = true)
    {
        switch (key.Status)
        {
            case KeyStatus.Revoked:
                return Result.Forbidden();
            case KeyStatus.Disabled:
                return Result.Success();
        }

        if (key.Server is not null)
            await _vpnRepository.DelKey(key.Server, key.Id, key.Protocol);
        
        key.Status = KeyStatus.Disabled;
        if (saveChanges) await _baseRepository.SaveContextAsync();
        return Result.Success();
    }
}