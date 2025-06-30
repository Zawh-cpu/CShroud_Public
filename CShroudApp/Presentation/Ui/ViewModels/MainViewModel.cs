using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Collections;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;
using CShroudApp.Presentation.Ui.DisplayItems;
using CShroudApp.Presentation.Ui.Interfaces;
using CShroudApp.Presentation.Ui.ViewModels.Auth;

using INotificationManager = CShroudApp.Core.Interfaces.INotificationManager;
using NotificationType = CShroudApp.Core.Entities.NotificationType;

namespace CShroudApp.Presentation.Ui.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentView = null!;
    
    private readonly INavigationService _navigationService;

    private const int MaxDisplayedNotificationsCount = 4;
    private const int MaxDisplayedHeaderNotificationsCount = 2;
    public AvaloniaList<NotificationDisplayItem> Notifications { get; } = new();
    
    public AvaloniaList<HeaderNotificationDisplayItem> HeaderNotifications { get; } = new();

    public MainViewModel(INavigationService navigationService, INotificationManager notificationManager, ISessionManager sessionManager)
    {
        _navigationService = navigationService;
        
        navigationService.ViewModelChanged += ChangeWindow;
        notificationManager.NotificationReceived += AddNotification;
        notificationManager.HeaderNotificationReceived += AddHeaderNotification;
        notificationManager.InternetConnectionRestored += () => Dispatcher.UIThread.Invoke(() =>
        {
            var notify = HeaderNotifications.FirstOrDefault(x => x.Reason == HeaderNotificationReason.InternetConnectionLost);
            if (notify is not null)
                Dispatcher.UIThread.Invoke(() => HeaderNotifications.Remove(notify));
        });;
        
        sessionManager.SessionHasBeenAuthorized += () => _navigationService.GoTo<DashboardViewModel>();
        
        try
        {
            if (sessionManager.RefreshToken is null)
                _navigationService.GoTo<LoginViewModel>();
            else
                _navigationService.GoTo<DashboardViewModel>();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        //#if DEBUG
        //        this.AttachDevTools(); // 👈 Включение инструментов отладки
        //#endif
    }

    //public MainViewModel()
    //{
    //    throw new NotImplementedException();
    //}

    public void AddNotification(NotificationObject notification)
    {
        if (Notifications.Count >= MaxDisplayedNotificationsCount)
        {
            var temp = Notifications[0];
            Task.Run(async () =>
            {
                await Dispatcher.UIThread.InvokeAsync(() => Notifications.Remove(temp));
                temp.Dispose();
            });
        }
        
        var notify = new NotificationDisplayItem()
        {
            Title = notification.Title,
            Message = notification.Message,
            Type = notification.Type,
        };

        notify.NotificationTimeOut += async () =>
        {
            await Dispatcher.UIThread.InvokeAsync(() => Notifications.Remove(notify));
            notify.Dispose();
        };

        notify.NotificationClicked += async () =>
        {
            await Dispatcher.UIThread.InvokeAsync(() => Notifications.Remove(notify));
            notify.Dispose();
        };
        
        Notifications.Add(notify);
        notify.StartCountdown(5000);
    }

    public void AddHeaderNotification(HeaderNotificationObject notification)
    {
        if (HeaderNotifications.Count >= MaxDisplayedHeaderNotificationsCount)
        {
            var temp = HeaderNotifications[0];
            Dispatcher.UIThread.Invoke(() => HeaderNotifications.Remove(temp));
        }
        
        var notify = new HeaderNotificationDisplayItem()
        {
            Message = notification.Message,
            Type = notification.Type,
            Reason = notification.Reason
        };
        
        notify.ClosedCommandEmit += async () =>
        {
            await Dispatcher.UIThread.InvokeAsync(() => HeaderNotifications.Remove(notify));
        };
        
        HeaderNotifications.Add(notify);
    }
    
    public void ChangeWindow(object? sender, ViewModelBase view)
    {
        CurrentView?.OnUnloaded();
        CurrentView = view;
        CurrentView?.OnLoaded();
    }
}