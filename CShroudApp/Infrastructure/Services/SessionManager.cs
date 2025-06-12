using Ardalis.Result;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Entities.User;
using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class SessionManager : ISessionManager
{
    private readonly IApiRepository _apiRepository;
    private readonly IStorageManager _storageManager;
    
    private User _session = User.Unauthenticated();
    public DateTime SessionExpires { get; private set; } = DateTime.MinValue;

    public event EventHandler? UnauthorizedSession;
    
    public User Session
    {
        get
        {
            if (SessionExpires <= DateTime.Now)
            {
                var result = Task.Run(UpdateSession).Result;
                if (result.IsSuccess)
                {
                    _session = result.Value;
                    SessionExpires = DateTime.Now.AddMinutes(60);
                }
                else
                {
                    UnauthorizedSession?.Invoke(this, EventArgs.Empty);
                    return User.Unauthenticated();
                }
            }
            
            return _session;
        }
        
        set => _session = value;
    }
    
    public string? RefreshToken
    {
        get => _apiRepository.RefreshToken;
        set
        {
            if (value is not null)
            {
                _storageManager.RefreshToken = value;
                _apiRepository.RefreshToken = value;
            }
        }
    }

    public string? ActionToken
    {
        set => _apiRepository.ActionToken = value;
    }

    public SessionManager(IApiRepository apiRepository, IStorageManager storageManager)
    {
        _apiRepository = apiRepository;
        _storageManager = storageManager;
        
        var token = _storageManager.RefreshToken;
        if (token is not null)
        {
            var parsedToken = Token.Parse(token);
            if (parsedToken.Expiration > DateTime.Now)
                RefreshToken = token;
        }
    }
    
    public async Task<Result<User>> UpdateSession()
    {
        var result = await _apiRepository.GetUserInformationAsync();
        if (!result.IsSuccess)
            return result.Map();
        
        return new User()
        {
            Id = result.Value.Id,
            IsVerified = result.Value.IsVerified,
            Nickname = result.Value.Nickname,
            Rate = result.Value.Rate,
            Role = result.Value.Role
        };
    }
}