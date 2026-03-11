using Ribbon.Helpers;
using System.Collections.ObjectModel;

namespace Ribbon.ViewModels;

public class RibbonTabViewModel : ObservableObject, IDisposable
{
    public RibbonViewModel Owner { get; internal set; }
    public ObservableCollection<RibbonGroupViewModel> Groups { get; } = [];

    public RibbonTabViewModel()
    {
        Groups.CollectionChanged += Groups_CollectionChanged;
    }

    public void AddRange(IEnumerable<RibbonGroupViewModel> items)
    {
        foreach (var item in items) 
            Groups.Add(item);
    }

    private void Groups_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (RibbonGroupViewModel item in e.NewItems)
                item.Owner = this;

        if (e.OldItems != null)
            foreach (RibbonGroupViewModel item in e.OldItems)
                item.Dispose();
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
        set => SetValue(ref field, value);
    }

    public void Dispose()
    {
        for(var i = 0; i < Groups.Count; i++)
        {
            Groups[i].Dispose();
        }

        Groups.Clear();
        Groups.CollectionChanged -= Groups_CollectionChanged;
    }
}
