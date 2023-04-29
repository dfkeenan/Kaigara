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

namespace Kaigara.ToolBars;
public class ToolBarItemGroupDefinition : ToolBarItemDefinition, IUIComponentDefinition, IEnumerable<ToolBarItemDefinition>
{
    private readonly ObservableCollection<ToolBarItemDefinition> items;

    public ToolBarItemGroupDefinition(string name, string? label = null, int displayOrder = 0) 
        : base(name, label, null, displayOrder)
    {
        items = new SortedObservableCollection<ToolBarItemDefinition>(UIComponentItemDefinition<ToolBarItemDefinition>.DisplayOrderComparer); ;
        Items = Collections.ObjectModel.ObservableCollectionExtensions.AsReadOnlyObservableCollection<ToolBarItemDefinition>(items);       
    }   

    public ReadOnlyObservableCollection<ToolBarItemDefinition> Items { get; }

    IEnumerable<IUIComponentDefinition> IUIComponentDefinition.Items => items;

    void IUIComponentDefinition.OnParentDefined(IUIComponentDefinition parent)
    {
        switch (parent)
        {
            case ToolBarDefinition toolBar:
                toolBar.Add(this);
                break;
            case ToolBarItemGroupDefinition toolBarItemGroup:
                toolBarItemGroup.Add(this);
                break;
        }
    }

    public void Add(ToolBarItemDefinition definition)
    {
        items.Add(definition);
    }

    public bool Remove(ToolBarItemDefinition definition)
    {
        return items.Remove(definition);
    }

    IEnumerator<ToolBarItemDefinition> IEnumerable<ToolBarItemDefinition>.GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    internal override IToolBarItemViewModel Build()
        => new DefinedToolBarItemGroupViewModel(this);
}