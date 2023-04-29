namespace Kaigara.ToolBars;

public class ToolBarItemDefinition : UIComponentItemDefinition<ToolBarItemDefinition>, IUIComponentDefinition
{
    public ToolBarItemDefinition(string name, string? label = null, string? iconName = null, int displayOrder = 0)
        : base(name, label, iconName, displayOrder)
    {
    }

    IEnumerable<IUIComponentDefinition> IUIComponentDefinition.Items => Enumerable.Empty<IUIComponentDefinition>();

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

    internal virtual IToolBarItemViewModel Build()
        => new DefinedToolBarItemViewModel(this);
}
