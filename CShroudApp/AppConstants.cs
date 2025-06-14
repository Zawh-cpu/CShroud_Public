namespace CShroudApp;

public static class AppConstants
{
    public static readonly string ApplicationDirectory = Directory.GetCurrentDirectory();
    
    public static readonly string WorkingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CrimsonShroud");
    
    public static readonly string ConfigFilePath = Path.Combine(WorkingDirectory, "config.json");
    
    public static readonly string CacheFilePath = Path.Combine(WorkingDirectory, "user.cache");
    
    public static readonly string InternalDataDirectory = Path.Combine(ApplicationDirectory, "Internal");
    public static readonly string InternalDirectIPsPath = Path.Combine(InternalDataDirectory, "InternalDirectIPs.txt");
    public static readonly string InternalDirectDomainsPath = Path.Combine(InternalDataDirectory, "InternalDirectDomains.txt");
    public static readonly string InternalGeoRulesPath = Path.Combine(InternalDataDirectory, "GeoRules", "SRSS");
    
    public static readonly string BinariesDirectory = Path.Combine(ApplicationDirectory, "Binaries");
}