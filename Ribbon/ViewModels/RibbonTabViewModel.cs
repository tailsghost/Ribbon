using Ribbon.Helpers;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Ribbon.ViewModels;

public class RibbonTabViewModel : ObservableObject, IDisposable
{
    internal RibbonViewModel? Owner;
    public ObservableCollection<RibbonGroupViewModel> Groups { get; } = [];

    public RibbonTabViewModel()
    {
        Groups.CollectionChanged += Groups_CollectionChanged;
    }

    private void Groups_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if(e.NewItems != null)
            foreach(RibbonGroupViewModel g in e.NewItems)
                g.Owner = Owner;
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

    public bool IsSelected
    {
        get => field;
        set
        {
            if(SetValue(ref field, value) && value)
            {
                Owner?.SelectedTab = this;
            }
        }
    }


    public void Dispose()
    {
        Owner = null;
        Groups.CollectionChanged -= Groups_CollectionChanged;
        foreach(var g in Groups)
            g.Dispose();
        Groups.Clear();
    }
}
