using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Kaigara.ViewModels;

namespace Kaigara.Menus
{
    public class MenuViewModel: IDisposable
    {
        private readonly MenuDefinition definition;
        private ObservableCollection<MenuItemViewModel> items;

        public MenuViewModel()
        {
            this.definition = CreateDefinition();
            items = definition.Items.ToObservableViewModelCollection(d => new MenuItemViewModel(d));
        }

        public MenuViewModel(MenuDefinition definition)
        {
            this.definition = definition ?? throw new ArgumentNullException(nameof(definition));
            items = definition.Items.ToObservableViewModelCollection(d => new MenuItemViewModel(d));
        }

        public IEnumerable<MenuItemViewModel> Items => items;

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