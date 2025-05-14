using System.Text;

namespace CShroudGateway.Infrastructure.Data.Config;

public class AuthConfig
{
    public string JwtIssuer { get; set; } = "ZWH";
    public string JwtAudience { get; set; } = "ZWH";
    public byte[] SecretKey { get; set; } = Encoding.UTF8.GetBytes("SparklingWaterInc");
}
