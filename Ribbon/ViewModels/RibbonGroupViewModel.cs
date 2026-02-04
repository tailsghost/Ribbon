using Ribbon.Helpers;
using System.Collections.ObjectModel;

namespace Ribbon.ViewModels;

public class RibbonGroupViewModel : ObservableObject, IDisposable
{
    internal RibbonViewModel? Owner;
    public ObservableCollection<RibbonButtonViewModel> Buttons { get; } = [];

    public RibbonGroupViewModel()
    {
        Buttons.CollectionChanged += Buttons_CollectionChanged;
    }

    public string Header
    {
        get => field;
        set => SetValue(ref field, value);
    } = string.Empty;

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

    private void Buttons_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (RibbonButtonViewModel g in e.NewItems)
                g.Owner = Owner;
    }

    public void Dispose()
    {
        Owner = null;
        Buttons.CollectionChanged -= Buttons_CollectionChanged;
        foreach (var item in Buttons)
            item.Dispose();
        Buttons.Clear();
    }
}
