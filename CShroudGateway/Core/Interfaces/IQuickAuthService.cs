using CShroudGateway.Core.Entities;

namespace CShroudGateway.Core.Interfaces;

public interface IQuickAuthService
{
    QuickAuthSession CreateSession();
    bool IsSessionExists(string sessionId);
    
    Task ConfirmSession(QuickAuthSession session, Guid userId);
    Task DeclineSession(QuickAuthSession session);
    Task PingSession(string sessionId);
    
    QuickAuthSession? GetSession(string sessionId);
    QuickAuthSession? FinalizeSession(string sessionId);
}