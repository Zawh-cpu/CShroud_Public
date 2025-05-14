﻿namespace CShroudApp.Core.Interfaces;

public interface IProcess
{
    void Start();
    void Kill();

    Task KillAsync();
    
    bool IsRunning { get; }
    
    public StreamWriter StandardInput { get; }
    
    event EventHandler ProcessExited;
    event EventHandler ProcessStarted;
}