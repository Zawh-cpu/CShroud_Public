using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CShroudApp.Core.Interfaces;
using CShroudApp.Core.Shared;
using CShroudApp.Infrastructure.Services;
using CShroudApp.Presentation.Interfaces;
using CShroudApp.Presentation.Services;
using CShroudApp.Presentation.Ui.ViewModels;
using CShroudApp.Presentation.Ui.ViewModels.Auth;
using CShroudApp.Presentation.Ui.Views;
using Microsoft.Extensions.DependencyInjection;

namespace CShroudApp.Presentation.Ui;

public partial class App : Avalonia.Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        IServiceProvider provider = SharedInAppMemory.ServiceProvider;
        if (provider is null)
        {
            provider = DependencyInjectionConfiguration.GetProvider();
        }
        
        var vm = provider.GetService<MainWindowViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainWindow()
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
        
        /*if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();*/
    }
}