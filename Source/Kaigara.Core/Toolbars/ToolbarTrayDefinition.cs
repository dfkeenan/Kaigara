using System.Collections;
using System.Collections.ObjectModel;
using Autofac;

namespace Kaigara.Toolbars;

public class ToolbarTrayDefinition : IEnumerable<ToolbarDefinition>, IUIComponentDefinition<ToolbarDefinition>, IUIComponentDefinition
{
    private readonly ObservableCollection<ToolbarDefinition> items;

    public ToolbarTrayDefinition(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        items = new ObservableCollection<ToolbarDefinition>();
        Items = Collections.ObjectModel.ObservableCollectionExtensions.AsReadOnlyObservableCollection<ToolbarDefinition>(items);
    }

    public string Name { get; }

    public ReadOnlyObservableCollection<ToolbarDefinition> Items { get; }

    public void Add(ToolbarDefinition definition)
    {
        items.Add(definition);
    }

    public bool Remove(ToolbarDefinition definition)
    {
        return items.Remove(definition);
    }

    IEnumerator<ToolbarDefinition> IEnumerable<ToolbarDefinition>.GetEnumerator()
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
