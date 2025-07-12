using System.Text.Json;
using CShroudApp.Core.Configs;
using CShroudApp.Core.Interfaces;
using CShroudApp.Core.JsonContexts;
using CShroudApp.Core.Utils;
using Microsoft.Extensions.Options;

namespace CShroudApp.Infrastructure.Services;

public class ConfigManager : IConfigManager
{
    public event Action? ConfigChanged;
    private readonly ApplicationConfig _applicationConfig;
    
    public ConfigManager(ApplicationConfig applicationConfig)
    {
        _applicationConfig = applicationConfig;
    }
    
    public void NotifyConfigChanged() => ConfigChanged?.Invoke();
    
    public async Task SaveConfigAsync()
    {
        FileChecker.CheckAndCreatePathToIfNotExists(AppConstants.ConfigFilePath);

        await File.WriteAllTextAsync(AppConstants.ConfigFilePath, JsonSerializer.Serialize(_applicationConfig, ConfigsJsonContext.Default.ApplicationConfig));
    }
}