using Kaigara.Commands;

namespace Kaigara.Menus;

public interface IMenuManager
{
    MenuViewModel BuildMenu(string name);
    IDisposable BuildMenu(MenuDefinition definition, out MenuViewModel menu);
    IDisposable ConfigureDefinition(MenuItemLocation location, Action<MenuItemDefinition> options);
    MenuItemDefinition CreateMenuItemDefinition(RegisteredCommandBase command, string name, string? label = null, string? iconName = null, int displayOrder = 0);
    MenuItemDefinition CreateMenuItemDefinition<TCommand>(string name, string? label = null, string? iconName = null, int displayOrder = 0) where TCommand : RegisteredCommandBase;
    IDisposable Register(MenuItemLocation location, MenuItemDefinition definition);
    IDisposable Register(MenuItemLocation location, params MenuItemDefinition[] definitions);
    IDisposable Register(MenuDefinition definition);
}
