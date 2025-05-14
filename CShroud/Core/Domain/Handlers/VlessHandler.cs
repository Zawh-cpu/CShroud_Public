using CShroud.Core.Domain.Interfaces;
using CShroud.Infrastructure.Interfaces;
using Xray.Common.Serial;

namespace CShroud.Core.Domain.Handlers;

public class VlessHandler : IProtocolHandler
{
    public TypedMessage MakeAccount(string uuid, Dictionary<string, string>? args)
    {
        return IVpnRepository.ToTypedMessage(new Xray.Proxy.Vless.Account()
        {
            Id = uuid,
            Flow = "xtls-rprx-vision",
            Encryption = "none"
        });
    }
}