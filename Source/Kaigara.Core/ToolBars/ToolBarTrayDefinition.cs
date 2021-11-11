using System.Collections;
using System.Collections.ObjectModel;
using Autofac;
using Kaigara.Collections.ObjectModel;

namespace Kaigara.ToolBars;

public class ToolBarTrayDefinition : IEnumerable<ToolBarDefinition>, IUIComponentDefinition<ToolBarDefinition>, IUIComponentDefinition
{
    private readonly ObservableCollection<ToolBarDefinition> items;

    public ToolBarTrayDefinition(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        items = new ObservableCollection<ToolBarDefinition>();
        Items = items.AsReadOnlyObservableCollection();
    }

    public string Name { get; }

    public ReadOnlyObservableCollection<ToolBarDefinition> Items { get; }

    public void Add(ToolBarDefinition definition)
    {
        items.Add(definition);
    }

    public bool Remove(ToolBarDefinition definition)
    {
        return items.Remove(definition);
    }

    IEnumerator<ToolBarDefinition> IEnumerable<ToolBarDefinition>.GetEnumerator()
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
