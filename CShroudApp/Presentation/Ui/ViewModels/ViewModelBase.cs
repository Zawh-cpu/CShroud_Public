using CommunityToolkit.Mvvm.ComponentModel;

namespace CShroudApp.Presentation.Ui.ViewModels;

public class ViewModelBase : ObservableObject
{
    protected CancellationTokenSource? CancellationTokenSource { get; private set; }
    
    public virtual void OnLoaded()
    {
        CancellationTokenSource = new CancellationTokenSource();
    }

    public virtual void OnUnloaded()
    {
        CancellationTokenSource?.Cancel();
        CancellationTokenSource?.Dispose();
        CancellationTokenSource = null;
    }
    
    public virtual void OnNavigated() {}
}