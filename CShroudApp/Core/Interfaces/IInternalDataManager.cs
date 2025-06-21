namespace CShroudApp.Core.Interfaces;

public interface IInternalDataManager
{
    public List<string> InternalDirectIPs { get; set; }
    public List<string> InternalDirectDomains { get; set; }
    public string[] InternalDirectProcesses { get; set; }
}