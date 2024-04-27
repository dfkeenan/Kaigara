using Kaigara.Commands;

namespace Kaigara.Toolbars;

public class ToolbarItemDefinition : UIComponentItemDefinition<ToolbarItemDefinition>, IUIComponentDefinition
{
    public ToolbarItemDefinition(string name, string? label = null, string? iconName = null, int displayOrder = 0, CanExecuteBehavior canExecuteBehavior = CanExecuteBehavior.Enabled)
        : base(name, label, iconName, displayOrder, canExecuteBehavior)
    {
    }

    IEnumerable<IUIComponentDefinition> IUIComponentDefinition.Items => Enumerable.Empty<IUIComponentDefinition>();

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

    internal virtual IToolbarItemViewModel Build()
        => new DefinedToolbarItemViewModel(this);
}
