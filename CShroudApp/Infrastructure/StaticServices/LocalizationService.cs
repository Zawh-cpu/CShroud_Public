using System.Text.Json;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;
using CShroudApp.Core.JsonContexts;

namespace CShroudApp.Infrastructure.StaticServices;

public static class LocalizationService
{
    private static readonly Dictionary<Localization, string> LocalizationFileNames = new()
    {
        [Localization.English] = "English.json",
        [Localization.Russian] = "Russian.json"
    };
    
    private static Dictionary<Localization, Dictionary<string, string>> LocalizationsBase { get; } = LoadLocalizationBase();
    private static readonly Dictionary<(Localization, string), string> TranslateCache = new();
    public static Localization CurrentLocalization { get; set; } = Localization.English;
    
    public static Dictionary<Localization, Dictionary<string, string>> LoadLocalizationBase()
    {
        Dictionary<Localization, Dictionary<string, string>> localBase = new();
        foreach (var localization in LocalizationFileNames.Keys)
        {
            var path = Path.Combine(AppConstants.InternalLocalizationFolderPath, LocalizationFileNames[localization]);
            if (!File.Exists(path)) continue;

            try
            {
                var local = JsonSerializer.Deserialize(File.ReadAllText(path),
                    ConfigsJsonContext.Default.DictionaryStringString);
                if (local is not null)
                    localBase[localization] = local;
            }
            catch (Exception)
            {
                // ignored
            }
        }
        
        return localBase;
    }
    
    public static string Translate(string key)
    {
        var cacheKey = (CurrentLocalization, key);
        if (TranslateCache.TryGetValue(cacheKey, out var cachedValue))
            return cachedValue;
        
        var value = LocalizationsBase.GetValueOrDefault(CurrentLocalization, new Dictionary<string, string>()).GetValueOrDefault(key, key);
        TranslateCache[cacheKey] = value;
        return value;
    }
    
    public static string Translate(string key, Localization localization)
    {
        var cacheKey = (localization, key);
        if (TranslateCache.TryGetValue(cacheKey, out var cachedValue))
            return cachedValue;
        
        var value = LocalizationsBase.GetValueOrDefault(localization, new Dictionary<string, string>()).GetValueOrDefault(key, key);
        TranslateCache[cacheKey] = value;
        return value;
    }
}