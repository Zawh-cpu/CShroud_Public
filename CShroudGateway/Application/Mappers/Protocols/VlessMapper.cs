namespace CShroudGateway.Application.Mappers.Protocols;

public static class VlessMapper
{
    public static string MakeOptions(Guid keyId)
    {
        return $"{{\"Id\": \"{keyId}\", \"Flow\": \"xtls-rprx-vision\", \"Encryption\": \"none\"}}";
    }
}