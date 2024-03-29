﻿using System.Collections;
using System.Collections.ObjectModel;
using Autofac;
using Kaigara.Collections.ObjectModel;

namespace Kaigara.Menus;

public class MenuDefinition : IEnumerable<MenuItemDefinition>, IUIComponentDefinition<MenuItemDefinition>, IUIComponentDefinition
{
    private readonly ObservableCollection<MenuItemDefinition> items;
    public MenuDefinition(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        items = new SortedObservableCollection<MenuItemDefinition>(UIComponentItemDefinition<MenuItemDefinition>.DisplayOrderComparer);
        Items = Collections.ObjectModel.ObservableCollectionExtensions.AsReadOnlyObservableCollection<MenuItemDefinition>(items);
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
