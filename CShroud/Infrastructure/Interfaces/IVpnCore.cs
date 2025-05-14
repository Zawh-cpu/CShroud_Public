namespace CShroud.Infrastructure.Interfaces;

public interface IVpnCore
{
    bool IsRunning { get; }
    void Start();
    
    
    event EventHandler VpnStopped;
    event EventHandler VpnStarted;
}