using Ribbon.Enums;
using Ribbon.Helpers;
using Ribbon.Interfaces;
using System.Collections.ObjectModel;

namespace Ribbon.ViewModels;

public class RibbonGroupViewModel : ObservableObject, IDisposable
{
    public RibbonTabViewModel Owner { get; internal set; }
    public ButtonActivationMode Mode { get; set; } = ButtonActivationMode.Momentary;
    public ObservableCollection<RibbonButtonViewModel> Buttons { get; } = [];

    public RibbonGroupViewModel()
    {
        Buttons.CollectionChanged += Buttons_CollectionChanged;
    }

    public void AddRange(IEnumerable<RibbonButtonViewModel> items)
    {
        foreach (var item in items) 
            Buttons.Add(item);
    }

    private void Buttons_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (RibbonButtonViewModel item in e.NewItems)
            {
                item.Owner = this;
                Owner?.Owner?.AddButton(item);
            }

        if (e.OldItems != null)
            foreach (RibbonButtonViewModel item in e.OldItems)
                item.Dispose();
    }

    private List<RibbonButtonViewModel> ActiveButtons { get; } = [];

    public List<RibbonButtonViewModel> GetActiveButtons() => [.. ActiveButtons];

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

    internal void AddActiveButton(RibbonButtonViewModel button)
    {
        if (Mode == ButtonActivationMode.Multiple)
            ActiveButtons.Add(button);
        if (Mode == ButtonActivationMode.Single)
        {
            if (ActiveButtons.Count == 0)
            {
                ActiveButtons.Add(button);
            }
            else
            {
                foreach (var ab in ActiveButtons) ab.IsSelected = false;
                ActiveButtons.Clear();
                ActiveButtons.Add(button);
            }
        }
        if(Mode == ButtonActivationMode.Momentary)
        {
            button.IsSelected = false;
        }
        if(Mode == ButtonActivationMode.None)
        {
            button.IsSelected = false;
        }
    }


    public void Dispose()
    {
        Owner = null;
        ActiveButtons.Clear();
        for(var i = 0;  i < Buttons.Count; i++)
        {
            var button = Buttons[i];
            button.Dispose();
        }
        Buttons.Clear();
        Buttons.CollectionChanged -= Buttons_CollectionChanged;
    }
}
