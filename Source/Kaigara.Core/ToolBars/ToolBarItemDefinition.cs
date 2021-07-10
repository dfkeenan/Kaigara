using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using Avalonia.Input;
using Kaigara.Collections.ObjectModel;
using Kaigara.Commands;
using ReactiveUI;

namespace Kaigara.ToolBars
{
    public class ToolBarItemDefinition : UIComponentItemDefinition<ToolBarItemDefinition>, IUIComponentDefinition
    {
        public ToolBarItemDefinition(string name, string? label = null, string? iconName = null) 
            : base(name, label, iconName)
        {
        }

        IEnumerable<IUIComponentDefinition> IUIComponentDefinition.Items => Enumerable.Empty<IUIComponentDefinition>();

        void IUIComponentDefinition.OnParentDefined(IUIComponentDefinition parent)
            => ((ToolBarDefinition)parent).Add(this);

        internal IToolBarItemViewModel Build()
            => new DefinedToolBarItemViewModel(this);
    }
}
