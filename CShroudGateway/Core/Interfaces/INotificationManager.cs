using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Core.Interfaces;

public interface INotificationManager
{
    void CallAndForget(List<Mail> notifications);
}