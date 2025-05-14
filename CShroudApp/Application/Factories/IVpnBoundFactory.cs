using CShroudApp.Application.Mappers;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Core.Shared.Dto;
using Newtonsoft.Json.Linq;

namespace CShroudApp.Application.Factories;

public static class IVpnBoundFactory
{
     private static readonly Dictionary<VpnProtocol, Func<JObject, IVpnBound>> Mappers = new()
     {
          { VpnProtocol.Vless, json => VlessMapper.ToDomain(json.ToObject<VlessDto>()!) },
     };

     public static IVpnBound CreateFromCredentials(VpnNetworkCredentials credentials)
     {

          // Проверяем, есть ли маппер для данного типа протокола
          if (Mappers.TryGetValue(credentials.Protocol, out var mapper))
          {
               return mapper(credentials.Credentials);
          }
        
          throw new InvalidOperationException("Unknown protocol type");
     }
}