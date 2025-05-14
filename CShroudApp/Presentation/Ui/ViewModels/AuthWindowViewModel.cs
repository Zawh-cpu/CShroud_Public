using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CShroudApp.Presentation.Ui.ViewModels;

public partial class AuthWindowViewModel : ViewModelBase
{
    private bool _isPasswordVisible = false;
    public char? PasswordChar => _isPasswordVisible ? null : '\u25cf';

    // Иконка в зависимости от состояния
    public string EyeIcon => _isPasswordVisible
        ? "/Assets/icons/svg/eye-open.svg"
        : "/Assets/icons/svg/eye-closed.svg";
    
    public ICommand TogglePasswordVisibilityCommand { get; }

    public AuthWindowViewModel()
    {
        TogglePasswordVisibilityCommand = new RelayCommand(() => ToggleVisibility());
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
}