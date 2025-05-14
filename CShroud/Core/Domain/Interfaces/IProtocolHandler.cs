using Xray.Common.Serial;

namespace CShroud.Core.Domain.Interfaces;

public interface IProtocolHandler
{
    public TypedMessage MakeAccount(string uuid, Dictionary<string, string>? args);
}