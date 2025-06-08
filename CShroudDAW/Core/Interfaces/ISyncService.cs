using Ardalis.Result;
using CShroudDAW.Application.DTOs;

namespace CShroudDAW.Core.Interfaces;

public interface ISyncService
{
    Task<Result<SyncResponseDto>> SyncKeys(string gatewayAddress, string secretKey);
}