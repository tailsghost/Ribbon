using Ribbon.Enums;
using Ribbon.Helpers;
using System.Windows.Input;
using System.Windows.Media;

namespace Ribbon.ViewModels;

public class RibbonButtonViewModel : ObservableObject, IDisposable
{
    public RibbonGroupViewModel Owner { get; internal set;  }
    public string Header
    {
        get => field;
        set => SetValue(ref field, value);
    } = string.Empty;

    public ImageSource Icon
    {
        get => field;
        set => SetValue(ref field, value);
    }

    public bool IsActive
    {
        get => field;
        set => SetValue(ref field, value);
    } = true;
    public bool IsVisible
    {
        get => field;
        set => SetValue(ref field, value);
    } = true;
    public bool IsSelected
    {
        get => field;
        set
        {
            if (SetValue(ref field, value) && value)
            {
                Owner.AddActiveButton(this);
            }
        }
    }

    public ICommand Command { get; set; }
    public object CommandParameter { get; set; }

    public void Dispose()
    {
        Command = null;
        CommandParameter = null;
        Owner = null;
    }
}
