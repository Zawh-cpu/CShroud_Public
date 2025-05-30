using DynamicData;
using ReactiveUI;
using System.Windows.Input;


using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Presentation.Interfaces;
using CShroudApp.Presentation.Services;
using CShroudApp.Presentation.Ui.ViewModels.Auth;

namespace CShroudApp.Presentation.Ui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ICommand ToAuthCommand { get; }
    
    /*public ViewModelBase CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }*/

    private ViewModelBase _currentView;
    
    public ViewModelBase CurrentView
    {
        get { return _currentView; }
        private set { this.RaiseAndSetIfChanged(ref _currentView, value); }
        //set { SetProperty(ref _currentView, value); }
    }

    public MainWindowViewModel(INavigationService navigationService)
    {
        // CurrentView = new AuthWindowViewModel();
        //CurrentView = new AuthWindowViewModel();

        navigationService.ViewModelChanged += ChangeWindow;
        
        // ToAuthCommand = new RelayCommand(() => ToAuth());
        //OnPropertyChanged(nameof(CurrentView));

        navigationService.GoTo<AuthViewModel>();
    }

    public void ChangeWindow(object? sender, ViewModelBase view)
    {
        CurrentView = view;
    }
}
