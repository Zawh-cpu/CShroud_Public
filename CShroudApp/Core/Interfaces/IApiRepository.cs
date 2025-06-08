using Ardalis.Result;
using CShroudApp.Application.DTOs;
using CShroudApp.Core.Entities.User;
using CShroudApp.Core.Entities.Vpn;

namespace CShroudApp.Core.Interfaces;

public interface IApiRepository
{
    Task<VpnNetworkCredentials?> ConnectToVpnNetworkAsync(List<VpnProtocol> supportedProtocols, string location);
    Task<Result<SignInDto>> FinalizeQuickAuthAttemptAsync(QuickAuthDto data);
    Task<Result<QuickAuthSessionDto>> CreateQuickAuthSessionAsync();
    Task<Result<User>> GetUserInformationAsync();
    Task<Result<ActionRefreshDto>> RefreshActionTokenAsync();
    
    public string RefreshToken { get; set; }
}