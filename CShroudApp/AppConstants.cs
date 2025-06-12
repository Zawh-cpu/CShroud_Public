namespace CShroudApp;

public static class AppConstants
{
    public static readonly string WorkingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CrimsonShroud");
    
    public static readonly string ConfigFilePath = Path.Combine(WorkingDirectory, "config.json");
    
    public static readonly string CacheFilePath = Path.Combine(WorkingDirectory, "user.cache");
}