using System.Diagnostics;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Processes;

public class BaseProcess : IProcess
{
    private Process _process;
    
    public bool IsRunning { get; private set; } = false;
    public bool HasExited { get; private set; } = false;

    public event EventHandler ProcessExited = delegate { };
    public event EventHandler ProcessStarted = delegate { };
    public StreamWriter StandardInput => _process.StandardInput;
    
    public BaseProcess(ProcessStartInfo processStartInfo, IProcessManager processManager, LogLevelMode debug = LogLevelMode.Off)
    {
        _process = new Process();
        _process.StartInfo = processStartInfo;
        _process.EnableRaisingEvents = true;
        _process.Exited += OnProcessExited;

        
        if (debug == LogLevelMode.Debug)
        {
            _process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine("[STDOUT] " + e.Data); // Print each line of standard output
                }
            };

            _process.ErrorDataReceived += ((sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine("[ERROR] " + e.Data);
                }
            });

        }
        
        processManager.Register(this);
        
    }

    private void ReactivateProcess()
    {
        Console.WriteLine("[REACTIVE PROCESS]");
        
        var startInfo = _process.StartInfo;
        
        _process = new Process();
        _process.StartInfo = startInfo;
        _process.EnableRaisingEvents = true;
        _process.Exited += OnProcessExited!;
    }
    
    public void Start(bool reactivateProcess = true)
    {
        if (HasExited && reactivateProcess)
            ReactivateProcess();
        
        IsRunning = true;
        ProcessStarted?.Invoke(this, EventArgs.Empty);
        
        _process.Start();
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();
    }

    public void Kill()
    {
        if (IsRunning || !_process.HasExited)
            _process.Kill();
    }

    public async Task KillAsync()
    {
        if (!IsRunning || _process.HasExited) return;
        _process.Kill();
        
        await _process.WaitForExitAsync();
    }
    
    private void OnProcessExited(object? sender, EventArgs? e)
    {
        IsRunning = false;
        HasExited = true;
        
        ProcessExited?.Invoke(this, EventArgs.Empty);
    }
}