using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class EventManager : IEventManager
{
    public event Action? AuthSessionExpired;
    public event EventHandler? VpnEnabled;
    public event EventHandler? VpnDisabled;
    
    public void CallAuthSessionExpired()
    {
        AuthSessionExpired?.Invoke();
    }
    
    public void CallVpnEnabled(object? sender, EventArgs e) => VpnEnabled?.Invoke(sender, e);
    public void CallVpnDisabled(object? sender, EventArgs e) => VpnDisabled?.Invoke(sender, e);
}