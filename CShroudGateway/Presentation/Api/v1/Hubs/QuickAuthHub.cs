using CShroudGateway.Core.Entities;
using CShroudGateway.Core.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CShroudGateway.Presentation.Api.v1.Hubs;

public class QuickAuthHub : Hub
{
    private readonly IQuickAuthService _quickAuthService;
    
    public QuickAuthHub(IQuickAuthService quickAuthService)
    {
        _quickAuthService = quickAuthService;
    }
    
    public async Task SubscribeToSession(string sessionId)
    {
        var session = _quickAuthService.GetSession(sessionId);
        
        if (session == null || (session is not null && (session.ExpiresAt < DateTime.UtcNow || session.Status != QuickAuthStatus.Pending)))
            throw new HubException("Session not found");
        
        await Groups.AddToGroupAsync(Context.ConnectionId, session!.SessionId);
    }
}