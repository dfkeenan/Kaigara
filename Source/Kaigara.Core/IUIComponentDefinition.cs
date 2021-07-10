using System;
using System.Collections.ObjectModel;
using Autofac;

namespace Kaigara
{
    public interface IUIComponentDefinition<TChild> : IDisposable
        where TChild : IUIComponentDefinition<TChild>
    {
        string Name { get; }
        ReadOnlyObservableCollection<TChild> Items { get; }
        void UpdateBindings(IComponentContext context);

        public void Add(TChild menuItemDefinition);

        public bool Remove(TChild menuItemDefinition);
    }
}
