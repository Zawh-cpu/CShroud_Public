namespace CShroudApp.Infrastructure.VpnCores.SingBox.Config;

public class ExperimentalObject
{
    public ExperimentalTempObject? CacheFile { get; set; }

    public class ExperimentalTempObject
    {
        public bool Enabled { get; set; } = true;
        public string? Path { get; set; }
    }
}