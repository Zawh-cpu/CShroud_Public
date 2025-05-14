using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class ProcessManager : IProcessManager
{
    private List<IProcess> _activeProcesses = new();
    private List<IProcess> _linkedProcesses = new();
    private List<IProcess> _elevatedProcesses = new();

    public void Register(IProcess process)
    {
        process.ProcessStarted += StartProcessEvent;
        process.ProcessExited += ExitProcessEvent;
        
        _linkedProcesses.Add(process);
    }

    public void KillAll()
    {
        foreach (var process in _activeProcesses)
        {
            process.Kill();
        }
    }

    public async Task KillAllAsync()
    {
        foreach (var process in _activeProcesses)
        {
            await process.KillAsync();
        }
    }

    private void StartProcessEvent(object? sender, EventArgs e)
    {
        if (sender == null) return;
        
        _activeProcesses.Add((sender as IProcess)!);
    }
    
    private void ExitProcessEvent(object? sender, EventArgs e)
    {
        if (sender == null) return;
        
        _activeProcesses.Remove((sender as IProcess)!);
    }
}