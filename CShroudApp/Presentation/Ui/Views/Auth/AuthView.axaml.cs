using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;

namespace CShroudApp.Presentation.Ui.Views.Auth;

public partial class AuthView : UserControl
{
    
    public AuthView()
    {
        InitializeComponent();
        //DataContext = new AuthWindowViewModel();
        //#if DEBUG
        //        this.AttachDevTools();
        //#endif
    }
}