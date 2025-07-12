namespace CShroudApp.Core.Interfaces;

public interface IConfigManager
{
    Task SaveConfigAsync();
    
    event Action? ConfigChanged;
    void NotifyConfigChanged();
}