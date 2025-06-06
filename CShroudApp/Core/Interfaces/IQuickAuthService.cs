using CShroudApp.Application.DTOs;
using Microsoft.AspNetCore.SignalR.Client;

namespace CShroudApp.Core.Interfaces;

public interface IQuickAuthService
{
    public event Action? OnAttemptDeclined;
    public event EventHandler<SignInDto>? OnAttemptSuccess;
    public event EventHandler<QuickAuthSessionDto>? OnSessionCreated;
    public event Action? OnSessionFailed;
    
    Task RunSession(CancellationToken cancellationToken);
}