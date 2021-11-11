using System.Reactive.Disposables;
using Autofac;
using Kaigara.Collections.Generic;

namespace Kaigara
{
    internal class UIComponentNode
    {
        private List<Action<IUIComponentDefinition>>? options;
        private Dictionary<string, UIComponentNode>? children;
        private IUIComponentDefinition? definition;
        private readonly string name;
        private readonly UIComponentGraph graph;


        public UIComponentNode(UIComponentGraph graph, string name)
        {
            this.graph = graph ?? throw new ArgumentNullException(nameof(graph));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public IUIComponentDefinition? Definition => definition;

        public IDisposable Add(UIComponentLocation location, IUIComponentDefinition definition)
        {
            var node = GetNode(location);
            node.AddDefinition(definition);

            return Disposable.Create(() =>
            {
                node.definition?.Dispose();
                node.definition = default;
            });
        }

        public IDisposable Add(IUIComponentDefinition definition)
        {
            var node = AddDefinition(definition);

            return Disposable.Create(() =>
            {
                node.definition?.Dispose();
                node.definition = default;
            });
        }

        public UIComponentNode GetNode(UIComponentLocation location)
        {
            var node = this;

            foreach (var name in location.PathSegments)
            {
                node = node.GetOrAddNode(name);
            }

            return node;
        }

        public IDisposable AddConfiguration(Action<IUIComponentDefinition> options)
        {
            this.options ??= new List<Action<IUIComponentDefinition>>();

            this.options.Add(options);

            return Disposable.Create(() =>
            {
                this.options.Remove(options);
            });
        }

        private UIComponentNode AddDefinition(IUIComponentDefinition definition)
        {
            var node = GetOrAddNode(definition.Name);
            node.SetDefinition(definition);
            return node;
        }

        private UIComponentNode GetOrAddNode(string name)
        {
            children ??= new Dictionary<string, UIComponentNode>();
            return children.GetOrAdd(name, n => new UIComponentNode(graph, n));
        }

        private void SetDefinition(IUIComponentDefinition definition)
        {
            if (this.definition is null)
            {
                this.definition = definition;
                if (children is not null)
                {
                    foreach (var item in children.Values.Where(n => n.definition is not null))
                    {
                        item.definition!.OnParentDefined(definition);
                    }
                }

                options?.ForEach(o => o(definition));

                definition.UpdateBindings(graph.ComponentContext);
            }

            foreach (var item in definition.Items)
            {
                GetOrAddNode(item.Name).SetDefinition(item);
            }
        }
    }
}
