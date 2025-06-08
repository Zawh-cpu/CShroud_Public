using System.Windows.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Core.Interfaces;

namespace CShroudApp.Presentation.Ui.ViewModels;

public partial class DashBoardViewModel : ViewModelBase
{
    private readonly ISessionManager _sessionManager;
    private readonly IEventManager _eventManager;
    private readonly IVpnService _vpnService;
    
    public DateTime Start = DateTime.UtcNow;
    private DispatcherTimer _timer;
    
    [ObservableProperty]
    private string _timerText = "00:00:00";
    
    public ICommand ToggleVpnCommand { get; }

    public bool temp = false;

    /*public DashBoardViewModel(ISessionManager sessionManager, IEventManager eventManager, IVpnService vpnService)
    {
        _sessionManager = sessionManager;
        _eventManager = eventManager;
        _vpnService = vpnService;
        
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        
        Console.WriteLine(_sessionManager.Session.Nickname);
        ToggleVpnCommand = new RelayCommand(ToggleVpn);
    }*/

    public void ToggleVpn()
    {
        if (temp)
            OnVpnDisabled();
        else
            OnVpnEnabled();
        
        temp = !temp;
    }

    private void OnVpnEnabled()
    {
        Start = DateTime.UtcNow;
        _timer.Start();
    }

    private void OnVpnDisabled()
    {
        _timer.Stop();
        TimerText = "00:00:00";
    }
    
    private void Timer_Tick(object? sender, EventArgs e)
    {
        var remaining = DateTime.UtcNow - Start;

        if (remaining <= TimeSpan.Zero)
        {
            TimerText = "00:00:00";
            _timer.Stop();
        }
        else
        {
            TimerText = $"{remaining.Hours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
        }
    }
}