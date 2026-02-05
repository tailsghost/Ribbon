using Ribbon.Commands;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xaml;

namespace Ribbon.Markup;

[MarkupExtensionReturnType(typeof(ICommand))]
public sealed class CommandRefExtension : MarkupExtension
{
    public string Path { get; }
    public string ParameterPath { get; set; }

    public CommandRefExtension(string path)
    {
        Path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var rootProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
        var rootObj = rootProvider?.RootObject;

        FrameworkElement rootFE;

        if (rootObj is not FrameworkElement root)
            rootFE = Application.Current?.MainWindow;
        else
            rootFE = root;


        if (rootFE == null)
            return null;

        var dc = rootFE.DataContext;
        if (dc == null)
            return null;

        object? current = dc;
        foreach (var part in Path.Split('.'))
        {
            if (current == null) break;
            var type = current.GetType();
            var prop = type.GetProperty(part, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            if (prop == null)
            {
                current = null;
                break;
            }
            current = prop.GetValue(current);
        }

        if (current == null)
            return null;

        if (current is not ICommand command)
            return null;

        object? parameter = null;
        if (!string.IsNullOrEmpty(ParameterPath))
        {
            current = dc;
            foreach (var part in ParameterPath.Split('.'))
            {
                if (current == null) break;
                var type = current.GetType();
                var prop = type.GetProperty(part, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                if (prop == null)
                {
                    current = null;
                    break;
                }
                current = prop.GetValue(current);
            }
            parameter = current;
        }

        if (parameter != null)
        {
            string commandName = command is RadTreeCommand rtc && !string.IsNullOrEmpty(rtc.CommandName)
                ? rtc.CommandName
                : Path;

            return new RelayCommand(
                commandName,
                () => command.Execute(parameter),
                () => command.CanExecute(parameter)
            );
        }

        return command;
    }
}
