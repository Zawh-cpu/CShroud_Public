using System.ComponentModel;
using Avalonia.Controls;

namespace CShroudApp.Presentation.Ui.Views;

public partial class MainView : Window
{
    public MainView()
    {
        //throw new Exception();
        InitializeComponent();
        this.Closing += OnClosing;
    }
    
    private void OnClosing(object? sender, CancelEventArgs e)
    {
        // Отменяем закрытие
        e.Cancel = true;

        // Прячем окно
        this.Hide();
    }
}