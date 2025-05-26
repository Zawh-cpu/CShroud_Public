using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using CShroudApp.Presentation.Ui.ViewModels;
using CShroudApp.Presentation.Ui.Views;

namespace CShroudApp.Presentation.Ui;

public partial class App : Avalonia.Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);
}