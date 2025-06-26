using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CShroudApp.Core.Entities;
using CShroudApp.Presentation.Ui.DisplayItems;

namespace CShroudApp.Presentation.Ui.Selectors;

public class NotificationTemplateSelector : AvaloniaObject, IDataTemplate
{
    public static readonly StyledProperty<IDataTemplate> InfoTemplateProperty =
        AvaloniaProperty.Register<NotificationTemplateSelector, IDataTemplate>(nameof(InfoTemplate));
    
    public static readonly StyledProperty<IDataTemplate> SuccessTemplateProperty =
        AvaloniaProperty.Register<NotificationTemplateSelector, IDataTemplate>(nameof(SuccessTemplate));
    
    public static readonly StyledProperty<IDataTemplate> WarningTemplateProperty =
        AvaloniaProperty.Register<NotificationTemplateSelector, IDataTemplate>(nameof(WarningTemplate));

    public static readonly StyledProperty<IDataTemplate> ErrorTemplateProperty =
        AvaloniaProperty.Register<NotificationTemplateSelector, IDataTemplate>(nameof(ErrorTemplate));

    public IDataTemplate InfoTemplate { get => GetValue(InfoTemplateProperty); set => SetValue(InfoTemplateProperty, value); }
    public IDataTemplate SuccessTemplate { get => GetValue(SuccessTemplateProperty); set => SetValue(SuccessTemplateProperty, value); }
    public IDataTemplate WarningTemplate { get => GetValue(WarningTemplateProperty); set => SetValue(WarningTemplateProperty, value); }
    public IDataTemplate ErrorTemplate { get => GetValue(ErrorTemplateProperty); set => SetValue(ErrorTemplateProperty, value); }
    

    public Control Build(object param)
    {
        if (param is NotificationDisplayItem notification)
        {
            return notification.Type switch
            {
                NotificationType.Info => (Control)InfoTemplate.Build(param),
                NotificationType.Success => (Control)SuccessTemplate.Build(param),
                NotificationType.Warning => (Control)WarningTemplate.Build(param),
                NotificationType.Error => (Control)ErrorTemplate.Build(param),
                _ => new TextBlock { Text = $"Unknown type: {notification.Type}" }
            };
        }

        return new TextBlock { Text = "Invalid object" };
    }

    public bool Match(object data) => data is NotificationDisplayItem;
}
