using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Autofac;

namespace Kaigara
{
    public interface IUIComponentDefinition<TChild>
        where TChild : IUIComponentDefinition
    {
        ReadOnlyObservableCollection<TChild> Items { get; }

        public void Add(TChild definition);

        public bool Remove(TChild definition);
    }

    public interface IUIComponentDefinition : IDisposable
    {
        string Name { get; }

        IEnumerable<IUIComponentDefinition> Items { get; }

        void OnParentDefined(IUIComponentDefinition parent);
        void UpdateBindings(IComponentContext context);
    }
}
