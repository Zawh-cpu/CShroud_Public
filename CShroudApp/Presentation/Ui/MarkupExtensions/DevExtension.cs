using Avalonia.Markup.Xaml;
using CShroudApp.Infrastructure.StaticServices;

namespace CShroudApp.Presentation.Ui.MarkupExtensions;

public class DevExtension : MarkupExtension
{
    public string Key { get; set; }

    public DevExtension(string key)
    {
        Key = key;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        #if DEBUG
            var prefix = $"beta-";
        #else
            var prefix = "";
        #endif
        
        var fileName = Path.GetFileNameWithoutExtension(Key); // "icon"
        var extension = Path.GetExtension(Key);               // ".ico"
        var fileWithExt = Path.GetFileName(Key);              // "icon.ico"
        var directory = Key.Substring(0, Key.Length - fileWithExt.Length);

        var newFileName = $"{prefix}{fileName}{extension}";        // "beta-icon.ico"
        
        Console.WriteLine(Path.Combine(directory, newFileName).Replace("\\", "/"));
        
        return Path.Combine(directory, newFileName).Replace("\\", "/");
    }
}