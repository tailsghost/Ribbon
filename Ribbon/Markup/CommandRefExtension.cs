using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xaml;

namespace Ribbon.Markup;

[MarkupExtensionReturnType(typeof(ICommand))]
public sealed class CommandRef : MarkupExtension
{
    public string Path { get; set; }

    public CommandRef(string path)
    {
        Path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var rootProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
        var rootObj = rootProvider?.RootObject;

        FrameworkElement rootFE = rootObj as FrameworkElement;

        if (rootFE == null)
            rootFE = Application.Current?.MainWindow;

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

        return current as ICommand;
    }
}
