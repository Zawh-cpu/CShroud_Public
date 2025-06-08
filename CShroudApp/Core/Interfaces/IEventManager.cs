namespace CShroudApp.Core.Interfaces;

public interface IEventManager
{
    public event Action? AuthSessionExpired;
    public event EventHandler? VpnEnabled;
    public event EventHandler? VpnDisabled;
    
    public void CallAuthSessionExpired();
    public void CallVpnEnabled(object? sender, EventArgs e);
    public void CallVpnDisabled(object? sender, EventArgs e);
}