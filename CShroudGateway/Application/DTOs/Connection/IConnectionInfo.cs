namespace CShroudGateway.Application.DTOs.Connection;

public interface IConnectionInfo
{
    public string Host { get; set; }
    public uint Port { get; set; }
}