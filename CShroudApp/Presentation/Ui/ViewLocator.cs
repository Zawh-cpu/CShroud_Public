using Avalonia.Controls;
using System;
using System.Collections.Generic;
using CShroudApp.Presentation.Ui.ViewModels;

namespace CShroudApp.Presentation.Ui;

public class ViewLocator
{
    private readonly Dictionary<Type, Func<Control>> _viewFactories = new();

    public ViewLocator()
    {
        // Явно регистрируем
        Register<AuthWindowViewModel>(() => new AuthWindowViewModel());
        // Register<DashboardViewModel>(() => new DashboardView());
    }

    public void Register<TViewModel>(Func<Control> factory)
    {
        _viewFactories[typeof(TViewModel)] = factory;
    }

    public Control Resolve(object viewModel)
    {
        var type = viewModel.GetType();
        if (_viewFactories.TryGetValue(type, out var factory))
            return factory();

        return new TextBlock { Text = "View not found for " + type.Name };
    }
}