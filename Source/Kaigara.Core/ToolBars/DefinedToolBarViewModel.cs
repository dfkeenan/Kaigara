using System.Collections.ObjectModel;
using Kaigara.Menus;
using System.Collections.Specialized;
using ReactiveUI;
using System.Collections;
using System.Windows.Input;
using Avalonia.Input;

namespace Kaigara.ToolBars;

internal class DefinedToolBarViewModel : ReactiveObject, IToolBarViewModel
{
    private ReadOnlyObservableCollection<IToolBarItemViewModel> items;
    private IDisposable changeSubscription;

    public DefinedToolBarViewModel(ToolBarDefinition definition)
    {
        Definition = definition ?? throw new ArgumentNullException(nameof(definition));

        items = Collections.ObjectModel.ObservableCollectionExtensions.ToReadOnlyObservableCollectionOf<ToolBarItemDefinition, IToolBarItemViewModel>(definition.Items, (Func<ToolBarItemDefinition, IToolBarItemViewModel>)(d => d.Build()));

        changeSubscription = definition.Changed.Subscribe(n =>
        {
            this.RaisePropertyChanged(n.PropertyName);
        });

        ((INotifyCollectionChanged)items).CollectionChanged += DefinedMenuItemViewModel_CollectionChanged;

        foreach (var item in items)
        {
            if (item is DefinedToolBarItemGroupViewModel group)
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
                if (item is DefinedToolBarItemGroupViewModel group)
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
                if (item is DefinedToolBarItemGroupViewModel group)
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

    public ToolBarDefinition Definition { get; }

    public string Name => Definition.Name;

    public bool IsVisible => Definition.IsVisible;

    public IEnumerable<IToolBarItemViewModel> Items
    {
        get
        {
            IToolBarItemViewModel? last = null;


            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (item is DefinedToolBarItemGroupViewModel group)
                {
                    if (group.IsVisible)
                    {
                        if (i > 0 && last != ToolBarItemSeparatorViewModel.Instance)
                        {
                            yield return last = ToolBarItemSeparatorViewModel.Instance;
                        }

                        foreach (var item2 in group.Items)
                            yield return last = item2;


                        if (i < items.Count - 1 && last != ToolBarItemSeparatorViewModel.Instance)
                        {
                            yield return last = ToolBarItemSeparatorViewModel.Instance;
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
                if (item is DefinedToolBarItemGroupViewModel group)
                {
                    group.ItemsChanged += CallItemsChanged;
                    group.PropertyChanged += Group_PropertyChanged;
                }

                item.Dispose();
            } 
        }
    }
}
