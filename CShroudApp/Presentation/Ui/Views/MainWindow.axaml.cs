using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using CShroudApp.Presentation.Ui.ViewModels;

namespace CShroudApp.Presentation.Ui.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        //DataContext = new MainWindowViewModel();
        
    #if DEBUG
            this.AttachDevTools();
    #endif
    }
}
