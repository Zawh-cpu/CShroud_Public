using System.Diagnostics;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;

namespace CShroudApp.Infrastructure.Services;

public class BaseProcess : IProcess
{
    private Process _process;
    public bool IsRunning => _isRunning;
    private bool _isRunning = false;
    private bool _hasExited = false;
    public bool HasExited => _hasExited;

    public event EventHandler ProcessExited = delegate { };
    public event EventHandler ProcessStarted = delegate { };
    public StreamWriter StandardInput => _process.StandardInput;
    
    private readonly IProcessManager _processManager;
    
    public BaseProcess(ProcessStartInfo processStartInfo,
        IProcessManager processManager, DebugMode debug = DebugMode.None)
    {
        _processManager = processManager;
        
        _process = new Process();
        _process.StartInfo = processStartInfo;
        _process.EnableRaisingEvents = true;
        _process.Exited += OnProcessExited!;

        if (debug != DebugMode.None)
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
        
        _processManager.Register(this);
        
    }

    public void Start()
    {
        _isRunning = true;
        ProcessStarted?.Invoke(this, EventArgs.Empty);
        _process.Start();
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();
    }

    public void Kill()
    {
        if (!_process.HasExited)
        {
            _process.Kill();
        }
    }

    public async Task KillAsync()
    {
        if (_process.HasExited) return;
        _process.Kill();
        await _process.WaitForExitAsync();
    }
    
    private void OnProcessExited(object sender, EventArgs e)
    {
        _isRunning = false;
        _hasExited = true;
        ProcessExited?.Invoke(this, EventArgs.Empty);
    }
}