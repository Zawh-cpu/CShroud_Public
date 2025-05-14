using System.Diagnostics;

namespace CShroud.Infrastructure.Interfaces;

public interface IProcess
{
    void Start();
    void Kill();
    
    bool IsRunning { get; }
    
    event EventHandler ProcessExited;
    event EventHandler ProcessStarted;
}