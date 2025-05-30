using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Core.Interfaces;

namespace CShroudApp.Presentation.Ui.ViewModels.Auth;

public partial class AuthViewModel : ViewModelBase
{
    private bool _isPasswordVisible = false;
    public char? PasswordChar => _isPasswordVisible ? null : '\u25cf';

    // Иконка в зависимости от состояния
    public string EyeIcon => _isPasswordVisible
        ? "/Assets/icons/svg/eye-open.svg"
        : "/Assets/icons/svg/eye-closed.svg";
    
    public ICommand TogglePasswordVisibilityCommand { get; }
    public ICommand TryFastLogin { get; }
    
    private IApiRepository _apiRepository;

    public AuthViewModel(IApiRepository apiRepository)
    {
        _apiRepository = apiRepository;
        
        TogglePasswordVisibilityCommand = new RelayCommand(() => ToggleVisibility());
        TryFastLogin = new RelayCommand(() => FastLoginAttempt());
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

    private async Task FastLoginAttempt()
    {
        Console.WriteLine("Fast login attempt");
        var r = await _apiRepository.TryFastLoginAsync();
        if (!r.IsSuccess)
        {
            // Create a fail notification manager
            Console.WriteLine("Fast login failed");
            return;
        }
        
        Console.WriteLine("Fast login success");
    }
}