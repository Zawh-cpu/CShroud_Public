using System.ComponentModel;
using Avalonia.Controls;
using CShroudApp.Presentation.Ui.ViewModels;

namespace CShroudApp.Presentation.Ui.Views;

public partial class AuthWindow : Window
{
    
    public AuthWindow()
    {
        InitializeComponent();
        DataContext = new AuthWindowViewModel();
    }
}