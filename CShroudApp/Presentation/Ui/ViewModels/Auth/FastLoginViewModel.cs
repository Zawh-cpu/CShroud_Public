using System.Collections.ObjectModel;
using ReactiveUI;

namespace CShroudApp.Presentation.Ui.ViewModels.Auth;

public class FastLoginViewModel : ViewModelBase
{
    // public ObservableCollection<string> MyItems { get; } = new ObservableCollection<string>() { "2", "2", "3" };
    
    private string _validCode;
    
    public string ValidCode
    {
        get { return _validCode; }
        private set { this.RaiseAndSetIfChanged(ref _validCode, value); }
        //set { SetProperty(ref _currentView, value); }
    }

    public void SetValidCode(string code)
    {
        ValidCode = code;
    }
}