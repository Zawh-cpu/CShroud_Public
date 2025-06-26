using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CShroudApp.Presentation.Ui.Interfaces;
using CShroudApp.Presentation.Ui.Services;
using CShroudApp.Presentation.Ui.ViewModels;
using CShroudApp.Presentation.Ui.ViewModels.Auth;
using CShroudApp.Presentation.Ui.Views;
using CShroudApp.Presentation.Ui.Views.Auth;
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
        var collection = new ServiceCollection();
        collection.AddSingleton<INavigationService, NavigationService>();
        
        collection.AddSingleton<MainViewModel>();
        collection.AddSingleton<LoginViewModel>();
        collection.AddSingleton<QuickLoginViewModel>();
        
        var host = BackendStarter.Start([], collection);
        try
        {

            var vm = host.Services.GetService<MainViewModel>();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (Design.IsDesignMode)
                {
                    desktop.MainWindow = new MainView()
                    {
                        DataContext = vm
                    };
                }
                else
                {
                    desktop.MainWindow = new MainView()
                    {
                        DataContext = vm
                    };
                }

            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                if (Design.IsDesignMode)
                {
                    singleViewPlatform.MainView = new MainView()
                    {
                        DataContext = new DesignerViewModel()
                    };
                }
                else
                {
                    singleViewPlatform.MainView = new MainView()
                    {
                        DataContext = vm
                    };
                }
            }

            base.OnFrameworkInitializationCompleted();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}