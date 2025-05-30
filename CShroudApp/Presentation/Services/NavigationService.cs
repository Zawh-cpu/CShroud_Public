using CShroudApp.Presentation.Interfaces;
using CShroudApp.Presentation.Ui.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CShroudApp.Presentation.Services;

public class NavigationService : INavigationService
{
    private IServiceProvider _serviceProvider;

    public event EventHandler<ViewModelBase>? ViewModelChanged;
    
    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public TViewModel GoTo<TViewModel>(params object[] args) where TViewModel : ViewModelBase
    {
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
        viewModel.SwapData(args);
        ViewModelChanged?.Invoke(this, viewModel);
        
        return viewModel;
        
    }
}