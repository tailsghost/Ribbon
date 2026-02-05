using Ribbon.Helpers;
using Ribbon.Interfaces;
using System.Collections.ObjectModel;

namespace Ribbon.ViewModels;

public class RibbonViewModel : ObservableObject, IDisposable
{
    public ObservableCollection<RibbonTabViewModel> Tabs { get; } = [];

    public RibbonViewModel()
    {
        Tabs.CollectionChanged += Tabs_CollectionChanged;
    }

    internal event Action<RibbonButtonViewModel> AddButtonAction;

    internal void AddButton(RibbonButtonViewModel button)
        => AddButtonAction?.Invoke(button);

    private void Tabs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (SelectedTab == null) {
            SelectedTab = Tabs[0];
        }

        if (e.NewItems != null)
            foreach (RibbonTabViewModel item in e.NewItems)
                item.Owner = this;

        if (e.OldItems != null)
            foreach (RibbonTabViewModel item in e.OldItems)
                item.Dispose();
    }

    public RibbonTabViewModel? SelectedTab
    {
        get => field;
        set
        {
            if (ReferenceEquals(field, value)) return;

            field?.IsSelected = false;
            field = value;
            field?.IsSelected = true;
            OnPropertyChanged();
        }
    }

    public void Dispose()
    {
        SelectedTab = null;
        Tabs.Clear();
        Tabs.CollectionChanged -= Tabs_CollectionChanged;
    }
}
