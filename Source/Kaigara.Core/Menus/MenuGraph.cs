using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using Kaigara.Collections.Generic;

namespace Kaigara.Menus
{
    class MenuGraph
    {
        private readonly MenuNode root = new MenuNode("ROOT");

        public IDisposable Add(MenuPath path, IMenuItem menuItem)
            => root.Add(path, menuItem);

        public IMenuItem? Find(MenuPath path)
            => root.GetNode(path).Item;

        private class MenuNode
        {
            private Dictionary<string, MenuNode>? children;
            private IMenuItem? item;
            private readonly string name;

            public MenuNode(string name)
            {
                this.name = name ?? throw new ArgumentNullException(nameof(name));
            }

            public IMenuItem? Item => item;

            public IDisposable Add(MenuPath path, IMenuItem menuItem)
            {
                var node = GetNode(path);
                node.item = menuItem;
                return Disposable.Create(() => node.item = null);
            }

            public MenuNode GetNode(MenuPath path)
            {
                var node = this;
                
                foreach (var name in path.PathSegments)
                {
                    node.children ??= new Dictionary<string, MenuNode>();
                    node = node.children.GetOrAdd(name, n => new MenuNode(n));
                }

                return node;
            }
        }
    }
}
