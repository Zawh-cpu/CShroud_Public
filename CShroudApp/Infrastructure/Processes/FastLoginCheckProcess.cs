using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Processes;

public class FastLoginCheckProcess : IProcess
{
    public void Start()
    {
        throw new NotImplementedException();
    }

    public void Kill()
    {
        throw new NotImplementedException();
    }

    public Task KillAsync()
    {
        throw new NotImplementedException();
    }

    public bool IsRunning { get; }
    public StreamWriter StandardInput { get; }
    public event EventHandler? ProcessExited;
    public event EventHandler? ProcessStarted;
}