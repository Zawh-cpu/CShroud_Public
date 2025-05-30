using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CShroudApp.Presentation.Ui.ViewModels;

namespace CShroudApp.Presentation.Ui;

public class ViewLocator : IDataTemplate
{
    public Control Build(object? data)
    {
        if (data is null)
        {
            return new TextBlock { Text = "data was null" };
        }
            
        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            Console.WriteLine("FOUND IT!");
            Console.WriteLine(data);
            Console.WriteLine(name);
            Console.WriteLine(type);
            return (Control)Activator.CreateInstance(type)!;
        }
        else
        {
            return new TextBlock { Text = "Not Found ---: " + name };
        }
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}