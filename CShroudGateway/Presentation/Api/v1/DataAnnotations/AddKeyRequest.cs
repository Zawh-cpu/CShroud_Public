using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CShroudGateway.Core.Entities;

namespace CShroudGateway.Presentation.Api.v1.DataAnnotations;

public class AddKeyRequest
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Required] public required VpnProtocol Protocol { get; init; }
    
    [Required] public required string Location { get; init; }
    public string? Name { get; init; }
}