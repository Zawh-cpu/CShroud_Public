using Ardalis.Result;

namespace CShroudDAW.Core.Interfaces;

public interface IVpnConnectionService
{
    Task<Result> AddConnection();
    Task<Result> RemoveConnection();
}