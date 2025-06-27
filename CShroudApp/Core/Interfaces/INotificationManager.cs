using CShroudApp.Core.Entities;

namespace CShroudApp.Core.Interfaces;

public interface INotificationManager
{
    void OnInternetInterrupt();
    void OnInternetConnectionRestored();
    void AddNotification(NotificationObject notification);
    void AddHeaderNotification(HeaderNotificationObject notification);

    
    event Action<NotificationObject>? NotificationReceived;
    event Action<HeaderNotificationObject>? HeaderNotificationReceived;
    event Action? InternetConnectionRestored;
    
    NotificationObject[] Notifications { get; }
    HeaderNotificationObject[] HeaderNotifications { get; }
}