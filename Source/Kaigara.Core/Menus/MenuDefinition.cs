using System.Collections;
using System.Collections.ObjectModel;
using Autofac;

namespace Kaigara.Menus;

public class MenuDefinition : IEnumerable<MenuItemDefinition>, IUIComponentDefinition<MenuItemDefinition>, IUIComponentDefinition
{
    private readonly ObservableCollection<MenuItemDefinition> items;
    public MenuDefinition(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        items = new ObservableCollection<MenuItemDefinition>();
        Items = Collections.ObjectModel.ReadOnlyObservableCollectionExtensionsHelpers.AsReadOnlyObservableCollection<MenuItemDefinition>(items);
    }

    public string Name { get; }

    public ReadOnlyObservableCollection<MenuItemDefinition> Items { get; }

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

    IEnumerable<IUIComponentDefinition> IUIComponentDefinition.Items => items;

    void IUIComponentDefinition.OnParentDefined(IUIComponentDefinition parent) { }

    void IUIComponentDefinition.UpdateBindings(IComponentContext context)
    {

    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {

    }
}
