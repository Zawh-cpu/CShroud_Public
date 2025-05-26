using CShroudApp.Presentation.Services;

namespace CShroudApp.Presentation.Ui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public NavigationService Navigation { get; }

    public MainWindowViewModel()
    {
        Navigation = new NavigationService();
    }
}
