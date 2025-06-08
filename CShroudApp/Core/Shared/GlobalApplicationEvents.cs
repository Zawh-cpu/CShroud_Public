namespace CShroudApp.Core.Shared;

public static class GlobalApplicationEvents
{
    public static event Action? SessionExpired;

    public static void InvokeSessionExpired()
    {
        SessionExpired?.Invoke();
    }
}