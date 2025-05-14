using Avalonia;
using CShroudApp.Application.Factories;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;
using CShroudApp.Infrastructure.Platforms.Linux.Services;
using CShroudApp.Infrastructure.Platforms.Windows.Services;
using CShroudApp.Infrastructure.Services;
using CShroudApp.Infrastructure.VpnLayers.SingBox;
using CShroudApp.Presentation.Ui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VpnCore = CShroudApp.Infrastructure.Services.VpnCore;

namespace CShroudApp;

internal static class Program
{
    static int Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var services = new ServiceCollection();

        services.Configure<SettingsConfig>(config.GetSection("Settings"));

        services.AddSingleton<IProcessManager, ProcessManager>();
        services.AddTransient<IProcessFactory, ProcessFactory>();
        services.AddSingleton<IApiRepository, ApiRepository>();
        services.AddSingleton<IVpnCore, VpnCore>();

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



        var service = services.BuildServiceProvider();

        var vpnService = service.GetRequiredService<IVpnService>();
        //vpnService.VpnEnabled += Aboba;
        //vpnService.VpnDisabled += AbobaOff;
        vpnService.EnableAsync(VpnMode.Tun).GetAwaiter().GetResult();
        //UiLoader.Run([]);
        while (true)
        {
    
        }

        return 0;
    }
    
    [STAThread]
    static void Run(string[] args) => UiLoader.Run(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    static AppBuilder BuildAvaloniaApp() => UiLoader.BuildAvaloniaApp();    
}