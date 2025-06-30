using System.Text.Json;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;
using CShroudApp.Core.JsonContexts;

namespace CShroudApp.Infrastructure.Services;

public class LocalizationService : ILocalizationService
{
    private static readonly Dictionary<Localization, string> LocalizationFileNames = new()
    {
        [Localization.English] = "English.json",
        [Localization.Russian] = "Russian.json"
    };
    
    private Dictionary<Localization, Dictionary<string, string>> LocalizationsBase { get; } = new();
    private readonly Dictionary<(Localization, string), string> _translateCache = new();
    
    public Localization CurrentLocalization { get; set; } = Localization.English;
    
    public LocalizationService()
    {
        foreach (var localization in LocalizationFileNames.Keys)
        {
            var path = Path.Combine(AppConstants.InternalLocalizationFolderPath, LocalizationFileNames[localization]);
            if (!File.Exists(path)) continue;

            try
            {
                var local = JsonSerializer.Deserialize(File.ReadAllText(path),
                    ConfigsJsonContext.Default.DictionaryStringString);
                if (local is not null)
                    LocalizationsBase[localization] = local;
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
    
    public string Translate(string key)
    {
        var cacheKey = (CurrentLocalization, key);
        if (_translateCache.TryGetValue(cacheKey, out var cachedValue))
            return cachedValue;
        
        var value = LocalizationsBase.GetValueOrDefault(CurrentLocalization, new Dictionary<string, string>()).GetValueOrDefault(key, key);
        _translateCache[cacheKey] = value;
        return value;
    }
    
    public string Translate(string key, Localization localization)
    {
        var cacheKey = (localization, key);
        if (_translateCache.TryGetValue(cacheKey, out var cachedValue))
            return cachedValue;
        
        var value = LocalizationsBase.GetValueOrDefault(localization, new Dictionary<string, string>()).GetValueOrDefault(key, key);
        _translateCache[cacheKey] = value;
        return value;
    }
}