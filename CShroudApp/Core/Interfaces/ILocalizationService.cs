using CShroudApp.Core.Entities;

namespace CShroudApp.Core.Interfaces;

public interface ILocalizationService
{
    string Translate(string key);
    string Translate(string key, Localization localization);
    
    Localization CurrentLocalization { get; set; }
}