using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;
using CShroudApp.Presentation.Ui.Interfaces;

namespace CShroudApp.Presentation.Ui.ViewModels.Auth;

public partial class LoginViewModel : ViewModelBase
{

    private bool _isPasswordVisible = false;
    public char? PasswordChar => _isPasswordVisible ? null : '\u25cf';

    // Иконка в зависимости от состояния
    public string EyeIcon => _isPasswordVisible
        ? "avares://CShroudApp/Assets/icons/svg/eye-open.svg"
        : "avares://CShroudApp/Assets/icons/svg/eye-closed.svg";
    
    public ICommand TogglePasswordVisibilityCommand { get; }
    public ICommand TryFastLogin { get; }
    public ICommand TryLoginUsingCredentialsCommand { get; }
    
    private readonly IApiRepository _apiRepository;
    private readonly INavigationService _navigationService;
    private readonly INotificationManager _notificationManager;

    public LoginViewModel(IApiRepository apiRepository, INavigationService navigationService, INotificationManager notificationManager)
    {
        _apiRepository = apiRepository;
        _navigationService = navigationService;
        _notificationManager = notificationManager;
        
        TogglePasswordVisibilityCommand = new RelayCommand(() => ToggleVisibility());
        TryFastLogin = new RelayCommand(() => QuickLoginAttempt());
        TryLoginUsingCredentialsCommand = new RelayCommand(() =>
        {
            _notificationManager.AddNotification(new NotificationObject()
            {
                Title = "Incomplete functionality",
                Message = "Sorry, but this functionality is disabled yet.",
                Type = NotificationType.Error
            });
        });
    }

    private void ToggleVisibility()
    {
        _isPasswordVisible = !_isPasswordVisible;
        //OnPropertyChanged(nameof(PasswordChar));
        //OnPropertyChanged(nameof(EyeIcon));
    }

    /*public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }*/

    private void QuickLoginAttempt()
    {
        _navigationService.GoTo<QuickLoginViewModel>();
    }
}