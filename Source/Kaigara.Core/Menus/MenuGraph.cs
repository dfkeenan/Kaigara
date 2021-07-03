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

        public IDisposable AddConfiguration(MenuPath path, Action<MenuItemDefinition> options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return root.GetNode(path).AddConfiguration(options);
        }

        private class MenuNode
        {
            private List<Action<MenuItemDefinition>>? options;
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
                node.Add(definition);                

                return Disposable.Create(() =>
                {
                    node.definition?.Dispose();
                    node.definition = null;
                });
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

            public IDisposable AddConfiguration(Action<MenuItemDefinition> options)
            {
                this.options ??= new List<Action<MenuItemDefinition>>();

                this.options.Add(options);

                return Disposable.Create(() =>
                {
                    this.options.Remove(options);
                });
            }

            private void Add(MenuItemDefinition definition)
            {
                var node = GetOrAddNode(definition.Name);
                node.SetDefinition(definition);
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

                    options?.ForEach(o => o(definition));

                    definition.UpdateBindings(graph.MenuManager.ComponentContext);
                }

                foreach (var item in definition.Items)
                {
                    GetOrAddNode(item.Name).SetDefinition(item);
                }
            }
        }
    }
}
