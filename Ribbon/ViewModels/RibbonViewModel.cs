using Ribbon.Helpers;
using System.Collections.ObjectModel;

namespace Ribbon.ViewModels;

public class RibbonViewModel : ObservableObject, IDisposable
{
    public ObservableCollection<RibbonTabViewModel> Tabs { get; } = [];

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

    public RibbonButtonViewModel? SelectedButton
    {
        get => field;
        set
        {
            if(ReferenceEquals(field, value)) return;

            field?.IsSelected = false;
            field = value;
            field?.IsSelected = true;
            OnPropertyChanged();
        }
    }

    public void Dispose()
    {
        SelectedButton = null;
        SelectedTab = null;
        foreach(var tab in Tabs) 
            tab.Dispose();
        Tabs.Clear();
    }
}
