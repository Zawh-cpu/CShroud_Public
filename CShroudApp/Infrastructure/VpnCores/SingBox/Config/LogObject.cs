namespace CShroudApp.Infrastructure.VpnCores.SingBox.Config;

public class LogObject
{
    public bool Disabled { get; set; } = false;
    public string Level { get; set; } = "info";
    public string? Output { get; set; }
    public bool Timestamp { get; set; } = true;
}