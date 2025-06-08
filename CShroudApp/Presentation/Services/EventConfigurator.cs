using CShroudApp.Core.Interfaces;
using CShroudApp.Presentation.Interfaces;
using CShroudApp.Presentation.Ui.ViewModels.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace CShroudApp.Presentation.Services;

public static class EventConfigurator
{
    public static void Configure(IServiceProvider serviceProvider)
    {
        var eventManager = serviceProvider.GetRequiredService<IEventManager>();
        var navigationService = serviceProvider.GetRequiredService<INavigationService>();
        var vpnService = serviceProvider.GetRequiredService<IVpnService>();
        
        eventManager.AuthSessionExpired += () => navigationService.GoTo<AuthViewModel>();

        vpnService.VpnEnabled += (sender, e) => eventManager.CallVpnEnabled(sender, e);
        vpnService.VpnDisabled += (sender, e) => eventManager.CallVpnDisabled(sender, e);
    }
}