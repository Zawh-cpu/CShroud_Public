namespace CShroudApp;

public static class GlobalConstants
{
    public static readonly string AppFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CrimsonShroud");
    public static readonly string DataFilePath = Path.Combine(AppFolderPath, "data.cache");
    public static readonly string ApplicationConfigPath = Path.Combine(AppFolderPath, "config.json");
}