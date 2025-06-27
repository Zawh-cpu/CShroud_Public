namespace CShroudApp.Core.Entities;

public enum HeaderNotificationReason
{
    InternetConnectionLost
}

public class HeaderNotificationObject
{
    public required string Message { get; set; }
    
    public required NotificationType Type { get; set; } = NotificationType.Success;
    public required HeaderNotificationReason Reason { get; set; }
}