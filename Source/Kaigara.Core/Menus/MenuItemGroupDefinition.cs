namespace Kaigara.Menus;

public class MenuItemGroupDefinition : MenuItemDefinition
{
    public MenuItemGroupDefinition(string name, string? label = null, int displayOrder = 0)
        : base(name, label, null, displayOrder)
    {
    }

    internal override IMenuItemViewModel Build()
        => new DefinedMenuItemGroupViewModel(this);
}
