using Ribbon.Commands;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
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

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
            return null;

        var rootProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
        if (rootProvider?.RootObject is not DependencyObject root)
            return null;

        ICommand? foundCommand = null;
        object? foundDc = null;

        Traverse(root, dc =>
        {
            var resolved = ResolvePath(dc, Path);
            if (resolved is not ICommand cmd) return false;
            foundCommand = cmd;
            foundDc = dc;
            return true;
        });

        if (foundCommand == null)
            return null;

        object? parameter = null;
        if (!string.IsNullOrEmpty(ParameterPath) && foundDc != null)
            parameter = ResolvePath(foundDc, ParameterPath);

        if (parameter != null)
        {
            return new RelayCommand(
                Path,
                () => foundCommand.Execute(parameter),
                () => foundCommand.CanExecute(parameter)
            );
        }

        return foundCommand;
    }

    private static void Traverse(
    DependencyObject root,
    Func<object, bool> tryDataContext)
    {
        void Walk(DependencyObject obj)
        {
            if (obj is FrameworkElement { DataContext: not null } fe)
            {
                if (tryDataContext(fe.DataContext))
                    throw new FoundException();
            }
            else if (obj is FrameworkContentElement { DataContext: not null } fce)
            {
                if (tryDataContext(fce.DataContext))
                    throw new FoundException();
            }

            if (obj is Visual or Visual3D)
            {
                var vCount = VisualTreeHelper.GetChildrenCount(obj);
                for (var i = 0; i < vCount; i++)
                {
                    Walk(VisualTreeHelper.GetChild(obj, i));
                }
            }

            foreach (var child in LogicalTreeHelper.GetChildren(obj))
            {
                if (child is DependencyObject dep)
                {
                    Walk(dep);
                }
            }
        }

        try
        {
            Walk(root);
        }
        catch (FoundException)
        {
        }
    }

    private sealed class FoundException : Exception { }
    private static object? ResolvePath(object source, string path)
    {
        if (source == null || string.IsNullOrEmpty(path)) return null;

        var current = source;
        foreach (var part in path.Split('.'))
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
        return current;
    }
}

