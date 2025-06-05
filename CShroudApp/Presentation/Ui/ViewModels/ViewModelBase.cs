using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace CShroudApp.Presentation.Ui.ViewModels;

public class ViewModelBase : ObservableObject
{
    protected CancellationTokenSource? CancellationTokenSource { get; private set; }
    
    public void SwapData(object[] args)
    {
        
    }

    public virtual void OnNavigated() {}
    
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
}