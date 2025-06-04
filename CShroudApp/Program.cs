using Avalonia;
using CShroudApp.Application.Factories;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Interfaces;
using CShroudApp.Core.Shared;
using CShroudApp.Infrastructure.Data.Config;
using CShroudApp.Infrastructure.Platforms.Linux.Services;
using CShroudApp.Infrastructure.Platforms.Windows.Services;
using CShroudApp.Infrastructure.Services;
using CShroudApp.Infrastructure.VpnLayers.SingBox;
using CShroudApp.Presentation.Interfaces;
using CShroudApp.Presentation.Services;
using CShroudApp.Presentation.Ui;
using CShroudApp.Presentation.Ui.ViewModels;
using CShroudApp.Presentation.Ui.ViewModels.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VpnCore = CShroudApp.Infrastructure.Services.VpnCore;

namespace CShroudApp;

internal static class Program
{
    static int Main(string[] args)
    {
        
        //

        var service = DependencyInjectionConfiguration.GetProvider();
        
        SharedInAppMemory.ServiceProvider = service;

        var vpnService = service.GetRequiredService<IVpnService>();
        //vpnService.VpnEnabled += Aboba;
        //vpnService.VpnDisabled += AbobaOff;
        //vpnService.EnableAsync(VpnMode.Disabled).GetAwaiter().GetResult();
        UiLoader.Run([]);

        return 0;
    }
    
    [STAThread]
    static void Run(string[] args) => UiLoader.Run(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    static AppBuilder BuildAvaloniaApp() => UiLoader.BuildAvaloniaApp();    
}