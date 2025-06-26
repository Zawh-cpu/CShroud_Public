namespace CShroudApp.Core.Entities;

public class NotificationObject
{
    public required string Title { get; set; }
    public required string Message { get; set; }
    public required NotificationType Type { get; set; } = NotificationType.Success;
}

public enum NotificationType
{
    Error,
    Warning,
    Success,
    Info
}