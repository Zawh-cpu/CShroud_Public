using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Core.Entities;
using CShroudApp.Presentation.Ui.DisplayItems;
using CShroudApp.Presentation.Ui.Interfaces;
using CShroudApp.Presentation.Ui.ViewModels.Auth;

namespace CShroudApp.Presentation.Ui.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentView = null!;
    
    public ICommand ToLoginCommand { get; }
    
    private readonly INavigationService _navigationService;

    public ObservableCollection<NotificationDisplayItem> Notifications { get; } = new();

    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        
        navigationService.ViewModelChanged += ChangeWindow;
        
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
        
        AddNotification(new NotificationObject()
        {
            Title = "Vpn started",
            Message = "Laleo lale lala",
            Type = NotificationType.Success,
        });
        
        AddNotification(new NotificationObject()
        {
            Title = "Vpn started",
            Message = "Laleo lale lala",
            Type = NotificationType.Info,
        });
        
        AddNotification(new NotificationObject()
        {
            Title = "Vpn started",
            Message = "Laleo lale lala",
            Type = NotificationType.Warning,
        });
        
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
        var notify = new NotificationDisplayItem()
        {
            Title = notification.Title,
            Message = notification.Message,
            Type = notification.Type,
        };
        
        Notifications.Add(notify);
    }
    
    public void ChangeWindow(object? sender, ViewModelBase view)
    {
        CurrentView?.OnUnloaded();
        CurrentView = view;
        CurrentView?.OnLoaded();
    }
}