using CShroudApp.Presentation.Services;
using ReactiveUI;


namespace CShroudApp.Presentation.Ui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private object _currentViewModel;

    public object CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }

    public MainWindowViewModel()
    {
        CurrentViewModel = new LoginViewModel();
    }
}
