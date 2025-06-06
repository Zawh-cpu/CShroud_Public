using System.Collections.Concurrent;
using System.Security.Cryptography;
using CShroudGateway.Core.Entities;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Presentation.Api.v1.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CShroudGateway.Infrastructure.Services;

public class QuickAuthServices : IQuickAuthService
{
    private string _letters =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    
    private readonly ConcurrentDictionary<string, QuickAuthSession> _sessions = new();
    private readonly IHubContext<QuickAuthHub> _hubContext;

    public QuickAuthServices(IHubContext<QuickAuthHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public QuickAuthSession CreateSession()
    {
        var session = new QuickAuthSession();
        _sessions[session.SessionId] = session;
        return session;
    }

    public bool IsSessionExists(string sessionId)
    {
        Console.WriteLine(string.Join(",", _sessions.Keys));
        return _sessions.ContainsKey(sessionId);
    }

    public async Task ConfirmSession(QuickAuthSession session, Guid userId)
    {
        session.Status = QuickAuthStatus.Confirmed;
        session.ConfirmedUserId = userId;
        session.SecretLoginCode = RandomNumberGenerator.GetString(_letters, 32);
        await _hubContext.Clients.Group(session.SessionId)
            .SendAsync("OnStatusChanged", new { Status = session.Status.ToString(), SessionId = session.SessionId, SecretLoginCode = session.SecretLoginCode });
    }
    
    public async Task DeclineSession(QuickAuthSession session)
    {
        session.Status = QuickAuthStatus.Declined;
        await _hubContext.Clients.Group(session.SessionId)
            .SendAsync("OnStatusChanged", new { Status = session.Status.ToString(), SessionId = session.SessionId });
    }

    public async Task PingSession(string sessionId)
    {
        await _hubContext.Clients.Group(sessionId)
            .SendAsync("OnStatusChanged", new { Status = "Pinged", SessionId = sessionId });
    }

    public QuickAuthSession? GetSession(string sessionId)
    {
        _sessions.TryGetValue(sessionId, out var session);
        return session;
    }

    public QuickAuthSession? FinalizeSession(string sessionId)
    {
        if (_sessions.TryGetValue(sessionId, out var session) && session.ExpiresAt > DateTime.UtcNow &&
            session.Status == QuickAuthStatus.Confirmed)
        {
            session.Status = QuickAuthStatus.Used;
            return session;
        }
        
        return null;
    }
}