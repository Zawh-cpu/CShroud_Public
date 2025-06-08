using System.ComponentModel.DataAnnotations;
using CShroudGateway.Core.Entities;
using CShroudGateway.Infrastructure.Services;

namespace CShroudGateway.Infrastructure.Data.Entities;

public class Server
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public required string Location { get; set; }
    
    [StringLength(21)]
    public required string IpV4Address { get; set; }
    
    [StringLength(49)]
    public required string IpV6Address { get; set; }

    public List<VpnProtocol> SupportedProtocols { get; set; } = new();
    public string SecretKey { get; set; } = Generator.GenerateRandomString(64);
}