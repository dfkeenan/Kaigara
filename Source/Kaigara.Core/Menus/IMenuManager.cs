using System;

namespace Kaigara.Menus
{
    public interface IMenuManager
    {
        MenuViewModel BuildMenu(string name);
        IDisposable BuildMenu(MenuDefinition definition, out MenuViewModel menu);
        IDisposable ConfigureMenuItemDefinition(MenuItemLocation path, Action<MenuItemDefinition> options);
        IDisposable Register(MenuItemLocation path, MenuItemDefinition definition);
        IDisposable Register(MenuDefinition definition);
    }
}