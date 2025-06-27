using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Collections;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Core.Entities;
using CShroudApp.Presentation.Ui.DisplayItems;
using CShroudApp.Presentation.Ui.Interfaces;
using CShroudApp.Presentation.Ui.ViewModels.Auth;

using INotificationManager = CShroudApp.Core.Interfaces.INotificationManager;

namespace CShroudApp.Presentation.Ui.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentView = null!;
    
    public ICommand ToLoginCommand { get; }
    
    private readonly INavigationService _navigationService;

    private const int MaxDisplayedNotificationsCound = 5;
    public AvaloniaList<NotificationDisplayItem> Notifications { get; } = new();

    public MainViewModel(INavigationService navigationService, INotificationManager notificationManager)
    {
        _navigationService = navigationService;
        
        navigationService.ViewModelChanged += ChangeWindow;
        notificationManager.NotificationReceived += AddNotification;
        

        ToLoginCommand = new RelayCommand(() =>
        {
            navigationService.GoTo<LoginViewModel>();
        });
        
        try
        {
            _navigationService.GoTo<LoginViewModel>();

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
        if (Notifications.Count >= MaxDisplayedNotificationsCound)
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
        Notifications.Add(notify);
        notify.StartCountdown(5000);
    }
    
    public void ChangeWindow(object? sender, ViewModelBase view)
    {
        CurrentView?.OnUnloaded();
        CurrentView = view;
        CurrentView?.OnLoaded();
    }
}