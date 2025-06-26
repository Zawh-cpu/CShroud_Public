using System.ComponentModel;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using CShroudApp.Presentation.Ui.DisplayItems;
using CShroudApp.Presentation.Ui.ViewModels;

namespace CShroudApp.Presentation.Ui.Views;

public partial class MainView : Window
{
    public MainView()
    {
        //throw new Exception();
        InitializeComponent();
        this.Closing += OnClosing;
#if DEBUG
        this.AttachDevTools();
#endif
    }
    
    private void OnClosing(object? sender, CancelEventArgs e)
    {
        // Отменяем закрытие
        e.Cancel = true;

        // Прячем окно
        this.Hide();
    }
}