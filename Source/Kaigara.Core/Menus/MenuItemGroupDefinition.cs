using Kaigara.Commands;

namespace Kaigara.Menus;

public class MenuItemGroupDefinition : MenuItemDefinition
{
    public MenuItemGroupDefinition(string name, string? label = null, int displayOrder = 0, CanExecuteBehavior canExecuteBehavior = CanExecuteBehavior.Enabled)
        : base(name, label, null, displayOrder, canExecuteBehavior)
    {
    }

    internal override IMenuItemViewModel Build()
        => new DefinedMenuItemGroupViewModel(this);
}
