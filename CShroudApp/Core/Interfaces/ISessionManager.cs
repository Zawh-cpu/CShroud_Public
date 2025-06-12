using Ardalis.Result;
using CShroudApp.Core.Entities.User;

namespace CShroudApp.Core.Interfaces;

public interface ISessionManager
{
    public User Session { get; }
    
    public DateTime SessionExpires { get; }
    public Task<Result<User>> UpdateSession();
    
    public string? RefreshToken { get; set; }
    public string? ActionToken { set; }

    event EventHandler? UnauthorizedSession;
}