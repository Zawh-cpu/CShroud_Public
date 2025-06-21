namespace CShroudApp.Core.Entities;

public enum ProxyProtocol
{
    Socks,
    Http
}

public record ProxyData(ProxyProtocol Protocol, string? Host, uint? Port, string[]? ExcludedHosts, bool Enabled);