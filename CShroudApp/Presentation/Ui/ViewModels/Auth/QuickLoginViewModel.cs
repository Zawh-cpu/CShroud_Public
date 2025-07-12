using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Application.DTOs;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Services;
using CShroudApp.Infrastructure.StaticServices;
using CShroudApp.Presentation.Ui.Interfaces;

namespace CShroudApp.Presentation.Ui.ViewModels.Auth;

public partial class QuickLoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _validCode = string.Empty;
    private QuickAuthSessionDto? _quickAuthSessionDto = null!;
    
    public ICommand OpenTelegramCommand { get; }
    public ICommand GoBackCommand { get; }
    
    private readonly INavigationService _navigationService;
    private readonly IApiRepository _apiRepository;
    private readonly IQuickAuthService _quickAuthService;
    private readonly ISessionManager _sessionManager;
    private readonly INotificationManager _notificationManager;
    
    public QuickLoginViewModel(INavigationService navigationService, IApiRepository apiRepository, IQuickAuthService quickAuthService, ISessionManager sessionManager, INotificationManager notificationManager)
    {
        _quickAuthService = quickAuthService;
        _quickAuthService.OnSessionCreated += OnSessionCreated;
        _quickAuthService.OnSessionFailed += OnSessionFailed;
        _quickAuthService.OnAttemptSuccess += OnAttemptSuccess;
        _quickAuthService.OnAttemptDeclined += OnAttemptDeclined;
        
        _navigationService = navigationService;
        _apiRepository = apiRepository;
        _sessionManager = sessionManager;
        _notificationManager = notificationManager;
        
        OpenTelegramCommand = new RelayCommand(() =>
        {
            if (_quickAuthSessionDto is not null)
                OpenTelegram(_quickAuthSessionDto.Variants, _quickAuthSessionDto.SessionId.ToString());
        });
        GoBackCommand = new RelayCommand(() => BackToAuth());
    }

    private void OnSessionCreated(object? sender, QuickAuthSessionDto session)
    {
        ValidCode = session.ValidVariant.ToString();
        _quickAuthSessionDto = session;
    }

    public override void OnNavigated()
    {
        var cts = new CancellationTokenSource();
        _quickAuthService.RunSession(cts.Token);
    }
    
    private void OnSessionFailed()
    {
        _notificationManager.AddNotification(new NotificationObject()
        {
            Title = LocalizationService.Translate("ErrorFailedStartQuickAuth"),
            Message = LocalizationService.Translate("ErrorFailedStartQuickAuth-Text"),
            Type = NotificationType.Error
        });
    }
    
    private void OnAttemptSuccess(object? sender, SignInDto session)
    {
        /*_notificationManager.AddNotification(new NotificationObject()
        {
            Title = _localizationService.Translate("QuickAuthSuccess"),
            Message = _localizationService.Translate("QuickAuthSuccess-Text"),
            Type = NotificationType.Success
        });*/
        
        _sessionManager.RefreshToken = session.RefreshToken;
        _navigationService.GoTo<DashboardViewModel>();
    }
    
    private void OnAttemptDeclined()
    {
        Console.WriteLine("OnAttemptDeclined");
    }

    private void BackToAuth()
    {
        _navigationService.GoTo<LoginViewModel>();
    }

    private void OpenTelegram(uint[] variants, string fastLoginId)
    {
        var data = $"verify_{fastLoginId}";
        
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