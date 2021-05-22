using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace Kaigara.Menus
{
    public class MenuManager : IMenuManager
    {
        private readonly List<MenuViewModel> menus = new List<MenuViewModel>();
        private readonly MenuGraph menuItemRegistrations = new MenuGraph();
        public IMenuItem? FindMenuItem(MenuPath path)
        {
            return menuItemRegistrations.Find(path);
        }

        public IDisposable Register(MenuPath path, IMenuItem menuItem)
        {
            return menuItemRegistrations.Add(path, menuItem);
        }

        public IDisposable Register(MenuViewModel menu)
        {
            if (menus.Contains(menu))
            {
                throw new ArgumentException("Menu already registered.", nameof(menu));
            }

            menus.Add(menu);

            return Disposable.Create(()=> menus.Remove(menu));
        }
    }
}
