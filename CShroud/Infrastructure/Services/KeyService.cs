using CShroud.Infrastructure.Interfaces;
using System.Collections.Generic;
using CShroud.Core.Domain.Interfaces;
using CShroud.Infrastructure.Data;
using CShroud.Infrastructure.Data.Entities;

namespace CShroud.Infrastructure.Services;

public class KeyService : IKeyService
{
    private readonly IVpnRepository _vpnRepository;
    private readonly IBaseRepository _baseRepository;
    private readonly IVpnCore _vpnCore;
    private HashSet<string> _activeKeys = new();
    
    public KeyService(IVpnRepository vpnRepository, IBaseRepository baseRepository, IVpnCore vpnCore)
    {
        _vpnRepository = vpnRepository;
        _baseRepository = baseRepository;
        _vpnCore = vpnCore;

        _vpnCore.VpnStarted += (object? sender, EventArgs e) => Task.Run(async() => await LoadActiveKeysOnStart(sender, e));
    }

    private async Task LoadActiveKeysOnStart(object? sender, EventArgs e)
    {
        await Task.Delay(4000);
        var context = new ApplicationContext();
        var users = await _baseRepository.GetAllActiveKeysByUserAsync(context);
        UInt32 c = 0;
        foreach (var user in users)
        {
            foreach (var key in user.Keys)
            {
                await _vpnRepository.AddKey(user.Rate!.VPNLevel, key.Uuid, key.ProtocolId);
                c++;
            }
        }
        
        Console.WriteLine($"[SYNCED] Loaded {c} active keys.");
    }
    
    public async Task<bool> EnableKey(User user, Key key)
    {
        if (!await _vpnRepository.AddKey(user.Rate!.VPNLevel, key.Uuid, key.ProtocolId)) return false;
        key.IsActive = true;
        
        return true;
    }

    public async Task<bool> DisableKey(Key key)
    {
        if (!await _vpnRepository.DelKey(key.Uuid, key.ProtocolId)) return false;
        key.IsActive = false;
        
        return true;
    }
}