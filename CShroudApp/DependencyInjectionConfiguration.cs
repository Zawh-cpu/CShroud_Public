using System.Text.Json;
using CShroudApp.Application.Factories;
using CShroudApp.Application.Serialization;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;
using CShroudApp.Infrastructure.Platforms.Linux.Services;
using CShroudApp.Infrastructure.Platforms.Windows.Services;
using CShroudApp.Infrastructure.Services;
using CShroudApp.Infrastructure.VpnLayers.SingBox;
using CShroudApp.Presentation.Interfaces;
using CShroudApp.Presentation.Services;
using CShroudApp.Presentation.Ui.ViewModels;
using CShroudApp.Presentation.Ui.ViewModels.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VpnCore = CShroudApp.Infrastructure.Services.VpnCore;

namespace CShroudApp;

public static class DependencyInjectionConfiguration
{
    private static IServiceProvider? _provider = null;
    private static ServiceCollection services = new ServiceCollection();

    public static IServiceProvider GetProvider()
    {
        if (_provider is not null) return _provider;
        
        var directory = Path.GetDirectoryName(GlobalConstants.DataFilePath);
        if (directory is not null && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        if (!File.Exists(GlobalConstants.ApplicationConfigPath))
        {
            Console.WriteLine("Application config file doesn't exist");
            var json = JsonSerializer.Serialize(SettingsConfig.MakeDefault(), AppJsonContext.Default.SettingsConfig);
        
            File.WriteAllText(GlobalConstants.ApplicationConfigPath, json);
        }
        
        Console.WriteLine(GlobalConstants.ApplicationConfigPath);
        
        var config = new ConfigurationBuilder()
            .AddJsonFile(GlobalConstants.ApplicationConfigPath, optional: false)
            .AddEnvironmentVariables()
            .Build();
        

        services.Configure<SettingsConfig>(config);

        services.AddSingleton<IProcessManager, ProcessManager>();
        services.AddTransient<IProcessFactory, ProcessFactory>();
        services.AddSingleton<IApiRepository, ApiRepository>();
        services.AddSingleton<IQuickAuthService, QuickAuthService>();
        services.AddSingleton<ISessionManager, SessionManager>();
        services.AddSingleton<IVpnCore, VpnCore>();
        services.AddSingleton<IStorageManager, StorageManager>();
        services.AddSingleton<IEventManager, EventManager>();

        switch (PlatformService.GetPlatform())
        {
            case "Windows": 
                services.AddSingleton<IProxyManager, WindowsProxyManager>();
                break;
    
            case "Linux":
                services.AddSingleton<IProxyManager, LinuxProxyManager>();
                break;
    
            default:
                Console.WriteLine("This platform currently is not supported. Sorry :(");
                Environment.Exit(1);
                break;
        }

        services.AddSingleton<IVpnService, VpnService>();


        // Optional
        services.AddSingleton<IVpnCoreLayer, SingBoxLayer>();
        
        services.AddHttpClient("CrimsonShroudApiHook",
            client => client.BaseAddress = new Uri("http://localhost:5271"));


        //
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<AuthViewModel>();
        services.AddTransient<FastLoginViewModel>();
        services.AddTransient<DashBoardViewModel>();
        
        var provider = services.BuildServiceProvider();
        
        _provider = provider;
        return provider;
    }
}