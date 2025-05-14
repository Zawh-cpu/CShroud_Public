using Ardalis.Result;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Core.Interfaces;

public interface IRateManager
{
    Task<Result> ChangeRateAsync(User user, bool saveChanges = true);
}