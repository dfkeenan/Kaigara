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
        private readonly MenuNode root;

        public MenuGraph(MenuManager menuManager)
        {
            root = new MenuNode(this, "ROOT");
            MenuManager = menuManager;
        }

        public MenuManager MenuManager { get; }

        public IDisposable Add(MenuPath path, MenuItemDefinition menuItemDefinition)
            => root.Add(path, menuItemDefinition);

        public MenuItemDefinition? Find(MenuPath path)
            => root.GetNode(path).Definition;

        private class MenuNode
        {
            private Dictionary<string, MenuNode>? children;
            private MenuItemDefinition? definition;
            private readonly string name;
            private readonly MenuGraph graph;

            public MenuNode(MenuGraph graph, string name)
            {
                this.graph = graph ?? throw new ArgumentNullException(nameof(graph));
                this.name = name ?? throw new ArgumentNullException(nameof(name));
            }

            public MenuItemDefinition? Definition => definition;

            public IDisposable Add(MenuPath path, MenuItemDefinition definition)
            {
                var node = GetNode(path);
                node.SetDefinition(definition);

                return Disposable.Create(() => node.definition = null);
            }

            public MenuNode GetNode(MenuPath path)
            {
                var node = this;
                
                foreach (var name in path.PathSegments)
                {
                    node = node.GetOrAddNode(name);
                }

                return node;
            }

            private MenuNode GetOrAddNode(string name)
            {
                children ??= new Dictionary<string, MenuNode>();
                return children.GetOrAdd(name, n => new MenuNode(graph, n));
            }

            private void SetDefinition(MenuItemDefinition definition)
            {
                if (this.definition is null)
                {
                    this.definition = definition;
                    if (children is not null)
                    {
                        foreach (var item in children.Values.Where(n => n.definition is not null))
                        {
                            definition.Add(item.definition!);
                        } 
                    }

                    if(definition is IRegisteredCommandMenuItemDefinition c)
                    {
                        c.BindCommand(graph.MenuManager.ComponentContext);
                    }
                }

                foreach (var item in definition.Items)
                {
                    GetOrAddNode(item.Name).SetDefinition(item);
                }
            }
        }
    }
}
