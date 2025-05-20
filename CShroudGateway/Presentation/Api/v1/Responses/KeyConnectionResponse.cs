using System.Text.Json;
using System.Text.Json.Serialization;
using CShroudGateway.Application.DTOs.Connection;
using CShroudGateway.Core.Entities;

namespace CShroudGateway.Presentation.Api.v1.Responses;

public class KeyConnectionResponse
{
    public required Guid Id { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required VpnProtocol Protocol { get; set; }
    public required object Credentials { get; set; }
}