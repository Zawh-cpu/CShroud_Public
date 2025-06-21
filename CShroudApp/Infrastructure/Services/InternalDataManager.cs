using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class InternalDataManager : IInternalDataManager
{
    public List<string> InternalDirectIPs { get; set; } = new();
    public List<string> InternalDirectDomains { get; set; } = new();
    public string[] InternalDirectProcesses { get; set; } = [];

    public InternalDataManager()
    {
        if (File.Exists(AppConstants.InternalDirectIPsPath))
            InternalDirectIPs.AddRange(File.ReadAllLines(AppConstants.InternalDirectIPsPath));
        
        if (File.Exists(AppConstants.InternalDirectDomainsPath))
            InternalDirectDomains.AddRange(File.ReadAllLines(AppConstants.InternalDirectDomainsPath));
        
        if (File.Exists(AppConstants.InternalDirectProcessesPath))
            InternalDirectDomains.AddRange(File.ReadAllLines(AppConstants.InternalDirectProcessesPath));
    }
}