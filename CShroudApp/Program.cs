using Avalonia;
using Avalonia.Logging;
using CShroudApp.Core.Utils;
using CShroudApp.Presentation.Ui;
using CShroudApp.Presentation.Ui.Interfaces;
using CShroudApp.Presentation.Ui.Services;
using CShroudApp.Presentation.Ui.ViewModels;
using CShroudApp.Presentation.Ui.ViewModels.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace CShroudApp;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        BackendStarter.Start([], App.GetUiDependencyCollection());
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace(LogEventLevel.Debug);
}