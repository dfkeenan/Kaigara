using System.Collections;
using System.Collections.ObjectModel;

namespace Kaigara.Menus;

public class MenuItemDefinition : UIComponentItemDefinition<MenuItemDefinition>, IUIComponentDefinition<MenuItemDefinition>, IUIComponentDefinition, IEnumerable<MenuItemDefinition>, IDisposable
{
    private readonly ObservableCollection<MenuItemDefinition> items;

    public MenuItemDefinition(string name, string? label = null, string? iconName = null)
        : base(name, label, iconName)
    {
        items = new ObservableCollection<MenuItemDefinition>();
        Items = Collections.ObjectModel.ReadOnlyObservableCollectionExtensionsHelpers.AsReadOnlyObservableCollection<MenuItemDefinition>(items);
    }

    public ReadOnlyObservableCollection<MenuItemDefinition> Items { get; }

    IEnumerable<IUIComponentDefinition> IUIComponentDefinition.Items => items;

    void IUIComponentDefinition.OnParentDefined(IUIComponentDefinition parent)
        => ((IUIComponentDefinition<MenuItemDefinition>)parent).Add(this);

    public void Add(MenuItemDefinition definition)
    {
        items.Add(definition);
    }

    public bool Remove(MenuItemDefinition definition)
    {
        return items.Remove(definition);
    }

    IEnumerator<MenuItemDefinition> IEnumerable<MenuItemDefinition>.GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    internal virtual IMenuItemViewModel Build()
        => new DefinedMenuItemViewModel(this);

}
