using System.Windows;
using System.Windows.Input;

namespace Ribbon.Models;

public class CommandModel : DependencyObject
{
    public string Header { get; set; }

    public static readonly DependencyProperty CommandProperty =
    DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(CommandModel));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(CommandModel));

    public ICommand CommandParameter
    {
        get => (ICommand)GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
}
