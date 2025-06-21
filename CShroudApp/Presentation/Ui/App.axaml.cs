using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
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
        
        var host = BackendStarter.Start([], collection);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            try
            {
                desktop.MainWindow = new MainView()
                {
                    DataContext = host.Services.GetRequiredService<MainViewModel>()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        base.OnFrameworkInitializationCompleted();
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