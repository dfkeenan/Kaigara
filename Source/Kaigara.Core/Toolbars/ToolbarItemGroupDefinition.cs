using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaigara.Collections.ObjectModel;
using Kaigara.Menus;
using ReactiveUI;

namespace Kaigara.Toolbars;
public class ToolbarItemGroupDefinition : ToolbarItemDefinition, IUIComponentDefinition, IEnumerable<ToolbarItemDefinition>
{
    private readonly ObservableCollection<ToolbarItemDefinition> items;

    public ToolbarItemGroupDefinition(string name, string? label = null, int displayOrder = 0) 
        : base(name, label, null, displayOrder)
    {
        items = new SortedObservableCollection<ToolbarItemDefinition>(UIComponentItemDefinition<ToolbarItemDefinition>.DisplayOrderComparer); ;
        Items = Collections.ObjectModel.ObservableCollectionExtensions.AsReadOnlyObservableCollection<ToolbarItemDefinition>(items);       
    }   

    public ReadOnlyObservableCollection<ToolbarItemDefinition> Items { get; }

    IEnumerable<IUIComponentDefinition> IUIComponentDefinition.Items => items;

    void IUIComponentDefinition.OnParentDefined(IUIComponentDefinition parent)
    {
        switch (parent)
        {
            case ToolbarDefinition toolBar:
                toolBar.Add(this);
                break;
            case ToolbarItemGroupDefinition toolBarItemGroup:
                toolBarItemGroup.Add(this);
                break;
        }
    }

    public void Add(ToolbarItemDefinition definition)
    {
        items.Add(definition);
    }

    public bool Remove(ToolbarItemDefinition definition)
    {
        return items.Remove(definition);
    }

    IEnumerator<ToolbarItemDefinition> IEnumerable<ToolbarItemDefinition>.GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    internal override IToolbarItemViewModel Build()
        => new DefinedToolbarItemGroupViewModel(this);
}