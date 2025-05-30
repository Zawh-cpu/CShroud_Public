using Ardalis.Result;
using CShroudApp.Application.DTOs;
using CShroudApp.Core.Entities.Vpn;

namespace CShroudApp.Core.Interfaces;

public interface IApiRepository
{
    Task<VpnNetworkCredentials?> ConnectToVpnNetworkAsync(List<VpnProtocol> supportedProtocols, string location);
    Task<Result<FastLoginDto>> TryFastLoginAsync();
}