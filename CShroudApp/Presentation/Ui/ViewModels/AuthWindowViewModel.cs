using System.ComponentModel;
using System.Reactive;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Presentation.Services;
using ExCSS;
using ReactiveUI;

namespace CShroudApp.Presentation.Ui.ViewModels;

public partial class AuthWindowViewModel : ViewModelBase
{
    private bool _isPasswordVisible = false;
    public char? PasswordChar => _isPasswordVisible ? null : '\u25cf';
    
    private readonly NavigationService _navigation;
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }
    

    // Иконка в зависимости от состояния
    public string EyeIcon => _isPasswordVisible
        ? "/Assets/icons/svg/eye-open.svg"
        : "/Assets/icons/svg/eye-closed.svg";
    
    public ICommand TogglePasswordVisibilityCommand { get; }

    public AuthWindowViewModel(NavigationService navigation)
    {
        TogglePasswordVisibilityCommand = new RelayCommand(() => ToggleVisibility());
        _navigation = navigation;
        LoginCommand = ReactiveCommand.Create(GoToDashboard);
    }

    private void ToggleVisibility()
    {
        _isPasswordVisible = !_isPasswordVisible;
        OnPropertyChanged(nameof(PasswordChar));
        OnPropertyChanged(nameof(EyeIcon));
    }

    /*public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }*/
    
    private void GoToDashboard()
    {
        _navigation.Navigate(new DashboardViewModel(_navigation));
    }
}