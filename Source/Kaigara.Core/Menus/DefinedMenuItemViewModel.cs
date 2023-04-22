using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ReactiveUI;

namespace Kaigara.Menus;

internal class DefinedMenuItemViewModel : DefinedMenuItemViewModelBase
{
    private ReadOnlyObservableCollection<IMenuItemViewModel> items;
    private IDisposable changeSubscription;

    private event Action? ItemsChanged;

    public DefinedMenuItemViewModel(MenuItemDefinition definition)
        : base(definition)
    {
        items = Collections.ObjectModel.ObservableCollectionExtensions.ToReadOnlyObservableCollectionOf<MenuItemDefinition, IMenuItemViewModel>(definition.Items, (Func<MenuItemDefinition, IMenuItemViewModel>)(d => d.Build()));

        ((INotifyCollectionChanged)items).CollectionChanged += DefinedMenuItemViewModel_CollectionChanged;

        foreach (var item in items)
        {
            if(item is DefinedMenuItemGroupViewModel group)
            {
                group.ItemsChanged += CallItemsChanged;
                group.PropertyChanged += Group_PropertyChanged;
            }
        }


        changeSubscription = definition.Changed.Subscribe(n =>
        {
            this.RaisePropertyChanged(n.PropertyName);
        });
    }

    private void Group_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        CallItemsChanged();
    }

    private void DefinedMenuItemViewModel_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if(e.OldItems is IList old)
        {
            foreach (var item in old)
            {
                if (item is DefinedMenuItemGroupViewModel group)
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
                if (item is DefinedMenuItemGroupViewModel group)
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

    public override IEnumerable<IMenuItemViewModel> Items
    {
        get
        {
            IMenuItemViewModel? last = null;


            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (item is DefinedMenuItemGroupViewModel group)
                {
                    if (group.IsVisible)
                    {
                        if (i > 0 && last != MenuItemSeparatorViewModel.Instance)
                        {
                            yield return last = MenuItemSeparatorViewModel.Instance;
                        }

                        foreach (var item2 in group.Items)
                            yield return last = item2;


                        if (i < items.Count - 1 && last != MenuItemSeparatorViewModel.Instance)
                        {
                            yield return last = MenuItemSeparatorViewModel.Instance;
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
        changeSubscription.Dispose();

        foreach (var item in items)
        {
            item.Dispose();
        }
    }
}
