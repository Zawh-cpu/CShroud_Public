namespace CShroudGateway.Core.Interfaces;

public interface IVpnStorage
{
    Dictionary<Guid, List<string>> Connections { get; set; }
}