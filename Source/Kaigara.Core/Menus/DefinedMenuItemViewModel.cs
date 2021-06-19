using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Kaigara.Collections.ObjectModel;

namespace Kaigara.Menus
{

    internal class DefinedMenuItemViewModel : DefinedMenuItemViewModelBase
    {
        private ReadOnlyObservableCollection<IMenuItemViewModel> items;
        private IDisposable changeSubscription;

        public DefinedMenuItemViewModel(MenuItemDefinition definition)
            :base(definition)
        {
            items = definition.Items.ToReadOnlyObservableCollectionOf(d => d.Build());

            changeSubscription = definition.Changed.Subscribe(n =>
            {
                this.RaisePropertyChanged(n.PropertyName);
            });
        }

        public override IEnumerable<IMenuItemViewModel> Items => items;

        public override void Dispose()
        {
            changeSubscription.Dispose();

            foreach (var item in items)
            {
                item.Dispose();
            }
        }
    }
}
