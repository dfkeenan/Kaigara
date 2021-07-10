using System;

namespace Kaigara.Menus
{
    public interface IMenuManager
    {
        MenuViewModel BuildMenu(string name);
        IDisposable BuildMenu(MenuDefinition definition, out MenuViewModel menu);
        IDisposable ConfigureDefinition(MenuItemLocation location, Action<MenuItemDefinition> options);
        IDisposable Register(MenuItemLocation location, MenuItemDefinition definition);
        IDisposable Register(MenuDefinition definition);
    }
}