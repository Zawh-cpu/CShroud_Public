using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CShroudApp.Core.Interfaces;
using CShroudApp.Presentation.Ui.Interfaces;
using CShroudApp.Presentation.Ui.MarkupExtensions;
using CShroudApp.Presentation.Ui.Services;
using CShroudApp.Presentation.Ui.ViewModels;
using CShroudApp.Presentation.Ui.ViewModels.Auth;
using CShroudApp.Presentation.Ui.Views;
using CShroudApp.Presentation.Ui.Views.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace CShroudApp.Presentation.Ui;


public partial class App : Avalonia.Application
{
    private IVpnService? _vpnService;

    public static ServiceCollection GetUiDependencyCollection()
    {
        var collection = new ServiceCollection();
        collection.AddSingleton<INavigationService, NavigationService>();
        
        collection.AddSingleton<MainViewModel>();
        collection.AddSingleton<LoginViewModel>();
        collection.AddSingleton<QuickLoginViewModel>();
        collection.AddSingleton<DashboardViewModel>();
        collection.AddSingleton<AppViewModel>();
        
        return collection;
    }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var host = BackendStarter.Start([], []);
        
        _vpnService = host.Services.GetRequiredService<IVpnService>();
        
        try
        {

            DataContext = host.Services.GetRequiredService<AppViewModel>();
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
                
                //Console.WriteLine("Apps exit configured");
                //desktop.Exit += OnApplicationExit;
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
            
            AppDomain.CurrentDomain.ProcessExit += OnEnvironmentExit;
            
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

    private void OnEnvironmentExit(object? sender, EventArgs e)
    {
        if (_vpnService is not null && _vpnService.IsRunning)
            Task.WaitAll(_vpnService.DisableAsync());
    }

    private void NativeTrayMenu_OnOpening(object? sender, EventArgs e)
    {
        Console.WriteLine("FWFWEFWEFWEFWFEWFEWFWEFEWFEWFEWFEWFFWEFWEFWF");
    }
}