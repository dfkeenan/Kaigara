using System;
using Autofac;

namespace Kaigara
{
    internal class UIComponentGraph<TDefinition,TLocation>
        where TDefinition : class, IUIComponentDefinition<TDefinition>
        where TLocation : UIComponentLocation
    {
        private readonly UIComponentNode<TDefinition, TLocation> root;

        public UIComponentGraph(IComponentContext context)
        {
            root = new UIComponentNode<TDefinition, TLocation>(this, "ROOT");
            ComponentContext = context;
        }

        internal IComponentContext ComponentContext { get; }
        public IDisposable Add(TLocation location, TDefinition definition)
            => root.Add(location, definition);

        public TDefinition? Find(TLocation location)
            => root.GetNode(location).Definition;

        public IDisposable AddConfiguration(TLocation location, Action<TDefinition> options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return root.GetNode(location).AddConfiguration(options);
        }
    }
}
