using DynamicData;
using ReactiveUI;
using System.Windows.Input;


using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Core.Interfaces;
using CShroudApp.Presentation.Interfaces;
using CShroudApp.Presentation.Services;
using CShroudApp.Presentation.Ui.ViewModels.Auth;

namespace CShroudApp.Presentation.Ui.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ICommand ToAuthCommand { get; }
    
    /*public ViewModelBase CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }*/

    [ObservableProperty]
    private ViewModelBase _currentView = null!;

    public MainWindowViewModel(INavigationService navigationService, ISessionManager sessionManager)
    {
        navigationService.ViewModelChanged += ChangeWindow;

        ToAuthCommand = new RelayCommand(() =>
        {
            navigationService.GoTo<AuthViewModel>();
        });

        if (sessionManager.RefreshToken is not null)
            navigationService.GoTo<DashBoardViewModel>();
        else
            navigationService.GoTo<AuthViewModel>();
    }

    public void ChangeWindow(object? sender, ViewModelBase view)
    {
        CurrentView?.OnUnloaded();
        CurrentView = view;
        CurrentView?.OnLoaded();
    }
}
