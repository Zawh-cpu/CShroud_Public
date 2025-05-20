using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using CShroudGateway.Core.Entities;

namespace CShroudGateway.Infrastructure.Data.Entities;

public class ProtocolSettings
{
    public required VpnProtocol Protocol { get; set; }
    
    [ForeignKey(nameof(Server))]
    
    public required Guid ServerId { get; set; }
    
    public Server? Server { get; set; }
    
    public required uint Port { get; set; }
    
    public required JsonDocument ExtraContent { get; set; }
    
}