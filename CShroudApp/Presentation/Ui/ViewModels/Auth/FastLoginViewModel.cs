using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Application.DTOs;
using CShroudApp.Core.Interfaces;
using CShroudApp.Presentation.Interfaces;
using ReactiveUI;

namespace CShroudApp.Presentation.Ui.ViewModels.Auth;

public partial class FastLoginViewModel : ViewModelBase
{
    // public ObservableCollection<string> MyItems { get; } = new ObservableCollection<string>() { "2", "2", "3" };
    
    [ObservableProperty]
    private string _validCode = string.Empty;
    private QuickAuthSessionDto _quickAuthSessionDto = null!;
    
    public ICommand OpenTelegramCommand { get; }
    public ICommand GoBackCommand { get; }
    
    private readonly INavigationService _navigationService;
    private readonly IApiRepository _apiRepository;
    private readonly IQuickAuthService _quickAuthService;
    
    public FastLoginViewModel(INavigationService navigationService, IApiRepository apiRepository, IQuickAuthService quickAuthService)
    {
        _quickAuthService = quickAuthService;
        _quickAuthService.OnSessionCreated += OnSessionCreated;
        _quickAuthService.OnSessionFailed += OnSessionFailed;
        
        _navigationService = navigationService;
        _apiRepository = apiRepository;
        
        OpenTelegramCommand = new RelayCommand(() => OpenTelegram(_quickAuthSessionDto.Variants, _quickAuthSessionDto.SessionId.ToString()));
        GoBackCommand = new RelayCommand(() => BackToAuth());
    }

    private void OnSessionCreated(object? sender, QuickAuthSessionDto session)
    {
        ValidCode = session.ValidVariant.ToString();
        _quickAuthSessionDto = session;
    }

    public override void OnNavigated()
    {
        Console.WriteLine("OnNavigated ---");
        var cts = new CancellationTokenSource();
        _quickAuthService.RunSession(cts.Token);
    }
    
    private void OnSessionFailed()
    {
        Console.WriteLine("fOnSessionFailed");
    }
    
    private void OnAttemptSuccess()
    {
        Console.WriteLine("OnAttemptSuccess");
    }
    
    private void OnAttemptDeclined()
    {
        Console.WriteLine("OnAttemptDeclined");
    }

    public void BackToAuth()
    {
        _navigationService.GoTo<AuthViewModel>();
    }

    public void OpenTelegram(uint[] variants, string fastLoginId)
    {
        var data = $"verify_{fastLoginId}_{string.Join("_", variants)}";
        
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
    }
}