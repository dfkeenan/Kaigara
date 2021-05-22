using System;

namespace Kaigara.Menus
{
    public interface IMenuManager
    {
        IMenuItem? FindMenuItem(MenuPath path);
        IDisposable Register(MenuPath path, IMenuItem menuItem);
        IDisposable Register(MenuViewModel menu);
    }
}