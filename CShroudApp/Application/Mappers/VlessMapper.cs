using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Core.Shared.Dto;

namespace CShroudApp.Application.Mappers;

public static class VlessMapper
{
    public static Vless ToDomain(VlessDto dto) => new Vless()
    {
        Host = dto.Host,
        Port = dto.Port,
        Uuid = dto.Uuid,
        Flow = dto.Flow,
        ServerName = dto.ServerName,
        Insecure = dto.Insecure,
        PublicKey = dto.PublicKey,
        ShortId = dto.ShortId,
    };
}