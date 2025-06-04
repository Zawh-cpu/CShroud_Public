using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Core.Interfaces;
using CShroudApp.Presentation.Interfaces;

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
    
    private readonly IApiRepository _apiRepository;
    private readonly INavigationService _navigationService;

    public AuthViewModel(IApiRepository apiRepository, INavigationService navigationService)
    {
        _apiRepository = apiRepository;
        _navigationService = navigationService;
        
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
        Console.WriteLine(r.Value.Id.ToString());
        Console.WriteLine(r.Value.ValidVariant.ToString());

        var data = $"verify_{r.Value.Id.ToString()}";
        
        var url = $"https://t.me/VeryRichBitchBot?start={Convert.ToBase64String(Encoding.UTF8.GetBytes(data))}";
        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true // Это важно!
            };
            process.Start();
        }
        catch (Exception ex)
        {
            // Тут можно вывести ошибку или залогировать
            Console.WriteLine($"Ошибка при открытии ссылки: {ex.Message}");
        }

        var a = _navigationService.GoTo<FastLoginViewModel>();
        a.SetValidCode(r.Value.ValidVariant.ToString());
    }
}