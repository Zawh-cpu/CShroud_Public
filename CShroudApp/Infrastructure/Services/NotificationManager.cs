using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class NotificationManager : INotificationManager
{
    private uint _currentIndex = 0;
    private const int NotificationArrayLenght = 50;
    
    public event Action<NotificationObject>? NotificationReceived;
    public NotificationObject[] Notifications { get; } = new NotificationObject[NotificationArrayLenght];
    
    public void OnInternetInterrupt()
    {
        //throw new NotImplementedException();
    }

    public void OnInternetConnectionRestored()
    {
        //throw new NotImplementedException();
    }

    public void AddNotification(NotificationObject notification)
    {
        if (_currentIndex >= NotificationArrayLenght) _currentIndex = 0;
        Notifications[_currentIndex] = notification;
        _currentIndex++;
        
        NotificationReceived?.Invoke(notification);
    }
}