using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class NotificationManager : INotificationManager
{
    public void OnInternetInterrupt()
    {
        throw new NotImplementedException();
    }

    public void OnInternetConnectionRestored()
    {
        throw new NotImplementedException();
    }
}