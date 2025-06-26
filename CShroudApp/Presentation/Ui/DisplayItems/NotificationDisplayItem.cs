using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Animation;
using CShroudApp.Core.Entities;

namespace CShroudApp.Presentation.Ui.DisplayItems;

public class NotificationDisplayItem : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public required string Title { get; set; }
    public required string Message { get; set; }
    public required NotificationType Type { get; set; } = NotificationType.Success;

    public CancellationTokenSource Cts { get; } = new CancellationTokenSource();
    public Action? OnCompleted { get; set; }

    public void NotificationHasExpired()
    {
        
    }
    
    protected void OnPropertyChanged([CallerMemberName] string? propName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}