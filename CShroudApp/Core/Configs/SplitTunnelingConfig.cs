namespace CShroudApp.Core.Configs;

public class SplitTunnelingConfig
{
    public bool Enabled { get; set; } = false;
    public string[] BypassIps { get; set; } = [];
    public string[] BypassHosts { get; set; } = [];
    public uint[] BypassPorts { get; set; } = [];
    public string[] BypassProcesses { get; set; } = [];
    public string[] BypassApps { get; set; } = [];
}