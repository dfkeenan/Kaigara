using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ReactiveUI;

namespace Kaigara.Toolbars;

internal class DefinedToolbarViewModel : ReactiveObject, IToolbarViewModel
{
    private ReadOnlyObservableCollection<IToolbarItemViewModel> items;
    private IDisposable changeSubscription;

    public DefinedToolbarViewModel(ToolbarDefinition definition)
    {
        Definition = definition ?? throw new ArgumentNullException(nameof(definition));

        items = Collections.ObjectModel.ObservableCollectionExtensions.ToReadOnlyObservableCollectionOf<ToolbarItemDefinition, IToolbarItemViewModel>(definition.Items, (Func<ToolbarItemDefinition, IToolbarItemViewModel>)(d => d.Build()));

        changeSubscription = definition.Changed.Subscribe(n =>
        {
            this.RaisePropertyChanged(n.PropertyName);
        });

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

    public ToolbarDefinition Definition { get; }

    public string Name => Definition.Name;

    public bool IsVisible => Definition.IsVisible;

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

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            changeSubscription.Dispose();

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
