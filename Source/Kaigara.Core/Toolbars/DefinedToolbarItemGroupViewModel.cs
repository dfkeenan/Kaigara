using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ReactiveUI;
using System.Collections;
using Avalonia.Input;
using System.Windows.Input;

namespace Kaigara.Toolbars;

internal class DefinedToolbarItemGroupViewModel : DefinedToolbarItemViewModel
{
    private ReadOnlyObservableCollection<IToolbarItemViewModel> items;

    internal event Action? ItemsChanged;

    public DefinedToolbarItemGroupViewModel(ToolbarItemGroupDefinition definition) : base(definition)
    {
        items = Collections.ObjectModel.ObservableCollectionExtensions.ToReadOnlyObservableCollectionOf<ToolbarItemDefinition, IToolbarItemViewModel>(definition.Items, (Func<ToolbarItemDefinition, IToolbarItemViewModel>)(d => d.Build()));

        ((INotifyCollectionChanged)items).CollectionChanged += DefinedMenuItemViewModel_CollectionChanged;

        foreach (var item in items)
        {
            if (item is DefinedToolbarItemGroupViewModel group)
            {
                group.ItemsChanged += CallItemsChanged;
                group.PropertyChanged += Group_PropertyChanged;
            }
        }
    }

    private void Group_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        CallItemsChanged();
    }

    private void DefinedMenuItemViewModel_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems is IList old)
        {
            foreach (var item in old)
            {
                if (item is DefinedToolbarItemGroupViewModel group)
                {
                    group.ItemsChanged -= CallItemsChanged;
                    group.PropertyChanged -= Group_PropertyChanged;
                }
            }
        }

        if (e.NewItems is IList items)
        {
            foreach (var item in items)
            {
                if (item is DefinedToolbarItemGroupViewModel group)
                {
                    group.ItemsChanged += CallItemsChanged;
                    group.PropertyChanged += Group_PropertyChanged;
                }
            }
        }

        this.RaisePropertyChanged(nameof(Items));
    }

    private void CallItemsChanged()
    {
        this.RaisePropertyChanged(nameof(Items));
    }

    public IEnumerable<IToolbarItemViewModel> Items
    {
        get
        {
            IToolbarItemViewModel? last = null;


            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (item is DefinedToolbarItemGroupViewModel group)
                {
                    if (group.IsVisible)
                    {
                        if (i > 0 && last != ToolbarItemSeparatorViewModel.Instance)
                        {
                            yield return last = ToolbarItemSeparatorViewModel.Instance;
                        }

                        foreach (var item2 in group.Items)
                            yield return last = item2;


                        if (i < items.Count - 1 && last != ToolbarItemSeparatorViewModel.Instance)
                        {
                            yield return last = ToolbarItemSeparatorViewModel.Instance;
                        }
                    }
                }
                else
                {
                    yield return item;
                }
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            foreach (var item in items)
            {
                if (item is DefinedToolbarItemGroupViewModel group)
                {
                    group.ItemsChanged += CallItemsChanged;
                    group.PropertyChanged += Group_PropertyChanged;
                }
                item.Dispose();
            }
        }
    }
}

public class ToolbarItemSeparatorViewModel : IToolbarItemViewModel
{
    public static ToolbarItemSeparatorViewModel Instance { get; } = new ToolbarItemSeparatorViewModel();

    private ToolbarItemSeparatorViewModel()
    {
        
    }

    public ICommand? Command => null;

    public object? CommandParameter => null;

    public KeyGesture? InputGesture => null;

    public bool IsVisible => true;

    public string? Label => "-";

    public string? IconName => null;

    public void Dispose()
    {

    }
}
