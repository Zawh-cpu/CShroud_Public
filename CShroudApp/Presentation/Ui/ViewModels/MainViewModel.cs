using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Presentation.Ui.Interfaces;
using CShroudApp.Presentation.Ui.ViewModels.Auth;

namespace CShroudApp.Presentation.Ui.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentView = null!;
    
    public ICommand ToLoginCommand { get; }
    
    private readonly INavigationService _navigationService;

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
        
        //#if DEBUG
        //        this.AttachDevTools(); // 👈 Включение инструментов отладки
        //#endif
    }

    //public MainViewModel()
    //{
    //    throw new NotImplementedException();
    //}

    public void ChangeWindow(object? sender, ViewModelBase view)
    {
        CurrentView?.OnUnloaded();
        CurrentView = view;
        CurrentView?.OnLoaded();
    }
}