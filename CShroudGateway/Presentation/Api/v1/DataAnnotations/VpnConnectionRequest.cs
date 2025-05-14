using System.ComponentModel.DataAnnotations;
using CShroudGateway.Core.Entities;

namespace CShroudGateway.Presentation.Api.v1.DataAnnotations;

public class VpnConnectionRequest
{
    [Required] public required string Location { get; set; }
    [Required] public required List<VpnProtocol> Protocols { get; set; }
}