using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Core.Entities;
using Timer = System.Timers.Timer;

namespace CShroudApp.Presentation.Ui.DisplayItems;

public partial class NotificationDisplayItem : INotifyPropertyChanged, IDisposable
{
    private Timer? _timer;
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public event Action? NotificationTimeOut;
    public event Action? NotificationClicked;
    
    public required string Title { get; set; }
    public required string Message { get; set; }
    public required NotificationType Type { get; set; } = NotificationType.Success;

    public void StartCountdown(uint time)
    {
        if (_timer != null) return;
        _timer = new Timer(time);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = false;
        _timer.Start();
    }

    private void OnTimedEvent(object? source, ElapsedEventArgs e) => NotificationTimeOut?.Invoke();
    
    protected void OnPropertyChanged([CallerMemberName] string? propName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
    
    private void Clicked(object? sender, PointerPressedEventArgs e) => NotificationClicked?.Invoke();

    public void Dispose()
    {
        if (_timer != null)
        {
            _timer.Stop();
            _timer?.Dispose();
        }
    }
}