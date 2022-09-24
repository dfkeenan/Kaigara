using System.Collections.ObjectModel;
using ReactiveUI;

namespace Kaigara.Menus;

internal class DefinedMenuItemViewModel : DefinedMenuItemViewModelBase
{
    private ReadOnlyObservableCollection<IMenuItemViewModel> items;
    private IDisposable changeSubscription;

    public DefinedMenuItemViewModel(MenuItemDefinition definition)
        : base(definition)
    {
        items = Collections.ObjectModel.ObservableCollectionExtensions.ToReadOnlyObservableCollectionOf<MenuItemDefinition, IMenuItemViewModel>(definition.Items, (Func<MenuItemDefinition, IMenuItemViewModel>)(d => d.Build()));

        changeSubscription = definition.Changed.Subscribe(n =>
        {
            this.RaisePropertyChanged(n.PropertyName);
        });
    }

    public override IEnumerable<IMenuItemViewModel> Items => items;

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
