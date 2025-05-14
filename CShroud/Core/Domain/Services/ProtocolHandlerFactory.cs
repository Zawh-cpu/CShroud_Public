using CShroud.Core.Domain.Interfaces;
using CShroud.Core.Domain.Handlers;

namespace CShroud.Core.Domain.Services;

public class ProtocolHandlerFactory : IProtocolHandlerFactory
{
    private Dictionary<string, Func<IProtocolHandler>> _handlers = new Dictionary<string, Func<IProtocolHandler>>()
    {
        { "vless", () => new VlessHandler() }
    };
    public bool Analyze(string protocolId, out Func<IProtocolHandler>? handler)
    {
        return _handlers.TryGetValue(protocolId, out handler);
    }
}