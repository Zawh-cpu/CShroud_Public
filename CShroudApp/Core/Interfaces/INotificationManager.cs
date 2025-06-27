using CShroudApp.Core.Entities;

namespace CShroudApp.Core.Interfaces;

public interface INotificationManager
{
    void OnInternetInterrupt();
    void OnInternetConnectionRestored();
    void AddNotification(NotificationObject notification);

    
    event Action<NotificationObject>? NotificationReceived;
    NotificationObject[] Notifications { get; }
}