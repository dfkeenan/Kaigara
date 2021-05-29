using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using Avalonia.Input;
using Kaigara.Commands;
using ReactiveUI;

namespace Kaigara.Menus
{
    public class MenuItemDefinition : ReactiveObject, IEnumerable<MenuItemDefinition>
    {
        private readonly ObservableCollection<MenuItemDefinition> items;
        public MenuItemDefinition(string name, string? label = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Label = label;
            items = new ObservableCollection<MenuItemDefinition>();
            Items = new ReadOnlyObservableCollection<MenuItemDefinition>(items);
        }

        public string Name { get; }

        public virtual string? Label { get; }


        public ReadOnlyObservableCollection<MenuItemDefinition> Items { get; }

        public virtual ICommand? Command => null;
        public virtual KeyGesture? InputGesture => null;

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
