namespace CShroud.Core.Domain.Interfaces;

public interface IProtocolHandlerFactory
{
    bool Analyze(string protocolId, out Func<IProtocolHandler>? protocolHandler);
}