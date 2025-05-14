namespace CShroudApp.Core.Entities;

public struct ProxyStruct
{
    public string Address { get; set; }
    public List<string> ExcludedHosts { get; set; }
    public bool Enabled { get; set; }
}