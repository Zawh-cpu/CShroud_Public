using System;
using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Core.Entities;

namespace CShroudApp.Presentation.Ui.DisplayItems;

public class HeaderNotificationDisplayItem
{
    private static readonly Dictionary<HeaderNotificationReason, string> _icons = new()
    {
        { HeaderNotificationReason.InternetConnectionLost, "ethernet-off.svg"}
    };
    
    public ICommand CloseNotificationCommand { get; }
    public event Action? ClosedCommandEmit;
    
    public required string Message { get; set; }
    
    public required NotificationType Type { get; set; } = NotificationType.Success;
    public required HeaderNotificationReason Reason { get; set; }

    public string NotificationIcon => _icons.GetValueOrDefault(Reason, string.Empty);

    public HeaderNotificationDisplayItem()
    {
        CloseNotificationCommand = new RelayCommand(() => ClosedCommandEmit?.Invoke());
    }
}