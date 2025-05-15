using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CShroudGateway.Core.Entities;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Presentation.Api.v1.Responses;

public class ServerKeyResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Location { get; set; }
    public required string Host { get; set; }
    public required uint Port { get; set; }
}

public class KeyResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public required ServerKeyResponse Server { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required VpnProtocol Protocol { get; set; }
    public required Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public KeyStatus Status { get; set; } = KeyStatus.Enabled;
}