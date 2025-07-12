using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.StaticServices;

namespace CShroudApp.Infrastructure.Services;

public class NotificationManager : INotificationManager
{
    private uint _currentIndex = 0;
    private uint _currentHeaderIndex = 0;
    
    private const int NotificationArrayLenght = 50;
    private const int HeaderNotificationArrayLenght = 5;
    
    public event Action<NotificationObject>? NotificationReceived;
    public event Action<HeaderNotificationObject>? HeaderNotificationReceived;
    
    public event Action? InternetConnectionRestored;
    public NotificationObject[] Notifications { get; } = new NotificationObject[NotificationArrayLenght];
    public HeaderNotificationObject[] HeaderNotifications { get; } = new HeaderNotificationObject[HeaderNotificationArrayLenght];
    
    private HeaderNotificationObject? _currentInternetOffNotification;
    
    public void OnInternetInterrupt()
    {
        if (_currentInternetOffNotification != null) return;
        
        _currentInternetOffNotification = new HeaderNotificationObject()
        {
            Type = NotificationType.Error,
            Message = LocalizationService.Translate("Error-InternetConnectionLost"),
            Reason = HeaderNotificationReason.InternetConnectionLost
        };
        
        AddHeaderNotification(_currentInternetOffNotification);
    }

    public void OnInternetConnectionRestored()
    {
        if (_currentInternetOffNotification == null) return;
        InternetConnectionRestored?.Invoke();
        _currentInternetOffNotification = null;
    }

    public void AddNotification(NotificationObject notification)
    {
        if (_currentIndex >= NotificationArrayLenght) _currentIndex = 0;
        Notifications[_currentIndex] = notification;
        _currentIndex++;
        
        NotificationReceived?.Invoke(notification);
    }
    
    public void AddHeaderNotification(HeaderNotificationObject notification)
    {
        if (_currentHeaderIndex >= HeaderNotificationArrayLenght) _currentHeaderIndex = 0;
        HeaderNotifications[_currentHeaderIndex] = notification;
        _currentHeaderIndex++;
        
        HeaderNotificationReceived?.Invoke(notification);
    }
}