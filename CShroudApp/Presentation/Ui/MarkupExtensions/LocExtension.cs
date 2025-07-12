using Avalonia.Markup.Xaml;
using CShroudApp.Infrastructure.StaticServices;

namespace CShroudApp.Presentation.Ui.MarkupExtensions;

public class LocExtension : MarkupExtension
{
    public string Key { get; set; }

    public LocExtension(string key)
    {
        Key = key;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return LocalizationService.Translate(Key);
    }
}