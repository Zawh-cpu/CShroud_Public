using System.Diagnostics;
using CShroud.Infrastructure.Interfaces;

namespace CShroud.Infrastructure.Services;

public class BaseProcess : IProcess
{
    private readonly Process _process;
    public bool IsRunning => _isRunning;
    private bool _isRunning = false;

    public event EventHandler ProcessExited = delegate { };
    public event EventHandler ProcessStarted = delegate { };

    public BaseProcess(ProcessStartInfo processStartInfo, bool debug=false)
    {
        _process = new Process();
        _process.StartInfo = processStartInfo;
        _process.Exited += OnProcessExited!;

        if (debug)
        {
            _process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine("[STDOUT] " + e.Data); // Print each line of standard output
                }
            };
        }
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
    
    /*
    private bool IsProcessRunning()
    {
        try
        {
            Console.WriteLine(_process.Responding);
            return _process.HasExited;
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("ERROR");
            return false;
        }
    }
    */
    
    private void OnProcessExited(object sender, EventArgs e)
    {
        _isRunning = false;
        ProcessExited?.Invoke(this, EventArgs.Empty);
    }
}