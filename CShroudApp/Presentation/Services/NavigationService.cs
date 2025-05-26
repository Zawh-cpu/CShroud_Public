using CShroudApp.Presentation.Ui.ViewModels;

namespace CShroudApp.Presentation.Services;

public class NavigationService : ReactiveObject
{
    private ViewModelBase _currentPage;
    public ViewModelBase CurrentPage
    {
        get => _currentPage;
        set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }

    public NavigationService()
    {
        // стартовая страница
        CurrentPage = new LoginViewModel(this);
    }

    public void Navigate(ViewModelBase viewModel)
    {
        CurrentPage = viewModel;
    }
}
