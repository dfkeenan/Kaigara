using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaigara.Collections.ObjectModel;

namespace Kaigara.Menus
{
    public class MenuDefinition : IEnumerable<MenuItemDefinition>
    {
        private readonly ObservableCollection<MenuItemDefinition> items;
        public MenuDefinition(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            items = new ObservableCollection<MenuItemDefinition>();
            Items = items.AsReadOnlyObservableCollection();
        }

        public string Name { get; }

        public ReadOnlyObservableCollection<MenuItemDefinition> Items { get; }

        public void Add(MenuItemDefinition menuItemDefinition)
        {
            items.Add(menuItemDefinition);
        }

        public bool Remove(MenuItemDefinition menuItemDefinition)
        {
            return items.Remove(menuItemDefinition);
        }

        IEnumerator<MenuItemDefinition> IEnumerable<MenuItemDefinition>.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}
