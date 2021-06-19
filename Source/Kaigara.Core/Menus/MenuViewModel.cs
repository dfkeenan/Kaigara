using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Kaigara.Collections.ObjectModel;

namespace Kaigara.Menus
{
    public class MenuViewModel: IDisposable
    {
        private readonly MenuDefinition definition;
        private ReadOnlyObservableCollection<IMenuItemViewModel> items;

        public MenuViewModel()
        {
            this.definition = CreateDefinition();
            items = definition.Items.ToReadOnlyObservableCollectionOf(d => d.Build());
        }

        public MenuViewModel(MenuDefinition definition)
        {
            this.definition = definition ?? throw new ArgumentNullException(nameof(definition));
            items = definition.Items.ToReadOnlyObservableCollectionOf(d => d.Build());
        }

        public IEnumerable<IMenuItemViewModel> Items => items;

        public MenuDefinition Definition => definition;

        protected virtual MenuDefinition CreateDefinition()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            foreach (var item in items)
            {
                item.Dispose();
            }
        }
    }
}