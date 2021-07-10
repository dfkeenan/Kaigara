using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using Autofac;
using Kaigara.Collections.Generic;

namespace Kaigara
{
    internal class UIComponentNode<TDefinition, TLocation>
        where TDefinition : class, IUIComponentDefinition<TDefinition>
        where TLocation : UIComponentLocation
    {
        private List<Action<TDefinition>>? options;
        private Dictionary<string, UIComponentNode<TDefinition, TLocation>>? children;
        private TDefinition? definition;
        private readonly string name;
        private readonly UIComponentGraph<TDefinition, TLocation> graph;


        public UIComponentNode(UIComponentGraph<TDefinition, TLocation> graph, string name)
        {
            this.graph = graph ?? throw new ArgumentNullException(nameof(graph));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public TDefinition? Definition => definition;

        public IDisposable Add(UIComponentLocation path, TDefinition definition)
        {
            var node = GetNode(path);
            node.Add(definition);

            return Disposable.Create(() =>
            {
                node.definition?.Dispose();
                node.definition = null;
            });
        }

        public UIComponentNode<TDefinition, TLocation> GetNode(UIComponentLocation path)
        {
            var node = this;

            foreach (var name in path.PathSegments)
            {
                node = node.GetOrAddNode(name);
            }

            return node;
        }

        public IDisposable AddConfiguration(Action<TDefinition> options)
        {
            this.options ??= new List<Action<TDefinition>>();

            this.options.Add(options);

            return Disposable.Create(() =>
            {
                this.options.Remove(options);
            });
        }

        private void Add(TDefinition definition)
        {
            var node = GetOrAddNode(definition.Name);
            node.SetDefinition(definition);
        }

        private UIComponentNode<TDefinition, TLocation> GetOrAddNode(string name)
        {
            children ??= new Dictionary<string, UIComponentNode<TDefinition, TLocation>>();
            return children.GetOrAdd(name, n => new UIComponentNode<TDefinition, TLocation>(graph, n));
        }

        private void SetDefinition(TDefinition definition)
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

                definition.UpdateBindings(graph.ComponentContext);
            }

            foreach (var item in definition.Items)
            {
                GetOrAddNode(item.Name).SetDefinition(item);
            }
        }
    }
}
