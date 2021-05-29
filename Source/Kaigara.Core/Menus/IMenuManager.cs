using System;

namespace Kaigara.Menus
{
    public interface IMenuManager
    {
        MenuViewModel BuildMenu(string name);
        IDisposable BuildMenu(MenuDefinition definition, out MenuViewModel menu);
        MenuItemDefinition? FindMenuItemDefinition(MenuPath path);
        IDisposable Register(MenuPath path, MenuItemDefinition definition);
        IDisposable Register(MenuDefinition definition);
    }
}