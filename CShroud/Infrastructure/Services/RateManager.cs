using CShroud.Infrastructure.Data;
using CShroud.Infrastructure.Data.Entities;
using CShroud.Infrastructure.Interfaces;

namespace CShroud.Infrastructure.Services;

public class RateManager : IRateManager
{
    IBaseRepository _baseRepository;
    IKeyService _keyService;
    
    public RateManager(IBaseRepository baseRepository, IKeyService keyService)
    {
        _baseRepository = baseRepository;
        _keyService = keyService;
    }
    
    public async Task UpdateRate(ApplicationContext context, User user)
    {

        if (user.Rate == null)
        {
            await _baseRepository.ExplicitLoadAsync(context, user, u => u.Rate!);
            if (await _baseRepository.CountKeysAsync(context, user.Id, active: true) <= user.Rate!.MaxKeys) return;
        }
        
        await _baseRepository.ExplicitLoadAsync(context, user, u => u.Keys!);
        
        if (user.Rate!.MaxKeys >= await _baseRepository.CountKeysAsync(context, user.Id)) return;
        for (var i = user.Keys!.Count - 1; i >= user.Rate!.MaxKeys; i--)
        {
            await _keyService.DisableKey(user.Keys![i]);
        }
    }
}