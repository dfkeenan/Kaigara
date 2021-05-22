using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Input;
using ReactiveUI;

namespace Kaigara.Menus
{
    public class MenuItemViewModel : ReactiveObject, IMenuItem, IEnumerable<IMenuItem>
    {
        private ObservableCollection<IMenuItem>? items;

        public MenuItemViewModel(string name, string label)
        {
            Name = name;
            Label = label;
        }

        public string Name { get; }

        public string Label { get; }

        public ObservableCollection<IMenuItem>? Items
        {
            get => items;
            private set => this.RaiseAndSetIfChanged(ref items, value);
        }

        public virtual ICommand? Command => null;
        public virtual KeyGesture? InputGesture => null;

        public void Add(IMenuItem menuItem)
        {
            items ??= new ObservableCollection<IMenuItem>();
            items.Add(menuItem);
        }

        public bool Remove(IMenuItem menuItem)
        {
            if(items is { })
            {
                return items.Remove(menuItem);
            }

            return false;
        }

        IEnumerator<IMenuItem> IEnumerable<IMenuItem>.GetEnumerator()
        {
            return (items ?? Enumerable.Empty<IMenuItem>()).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (items ?? Enumerable.Empty<IMenuItem>()).GetEnumerator();
        }

        ICollection<IMenuItem>? IMenuItem.Items => Items ??= new ObservableCollection<IMenuItem>();
    }
}
