using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Application.DTOs.Keys;

public record KeyWithVpnLevel(Key Key, uint VpnLevel);