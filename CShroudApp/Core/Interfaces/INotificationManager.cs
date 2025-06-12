namespace CShroudApp.Core.Interfaces;

public interface INotificationManager
{
    void OnInternetInterrupt();
    void OnInternetConnectionRestored();
}