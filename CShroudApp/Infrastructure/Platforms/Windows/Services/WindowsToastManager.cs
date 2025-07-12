using CShroudApp.Core.Interfaces;
using Microsoft.Toolkit.Uwp.Notifications;

namespace CShroudApp.Infrastructure.Platforms.Windows.Services;

public class WindowsToastManager : IToastManager
{
    public void SendToast()
    {
        Console.WriteLine("TOasttwetewt");
        var a = new ToastContentBuilder()
            .AddArgument("action", "viewConversation")
            .AddArgument("conversationId", 9813)
            .AddText("Andrew sent you a picture")
            .AddText("Check this out, The Enchantments in Washington!");
    }
}