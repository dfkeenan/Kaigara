namespace Kaigara.Menus;

public class MenuItemGroupDefinition : MenuItemDefinition
{
    public MenuItemGroupDefinition(string name, string? label = null)
        : base(name, label)
    {
    }

    internal override IMenuItemViewModel Build()
        => new DefinedMenuItemGroupViewModel(this);
}
