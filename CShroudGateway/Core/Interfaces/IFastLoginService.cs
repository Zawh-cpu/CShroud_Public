using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Core.Interfaces;

public interface IFastLoginService
{
    public FastLogin MakeSession(string? userAgent, string? ipAddress);
}