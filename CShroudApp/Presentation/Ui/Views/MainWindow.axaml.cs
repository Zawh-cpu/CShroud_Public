using Avalonia;
using Avalonia.Controls;

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
