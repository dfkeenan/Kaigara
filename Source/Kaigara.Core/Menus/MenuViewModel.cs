using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Kaigara.Menus
{
    public abstract class MenuViewModel
    {
        private ObservableCollection<IMenuItem>? items;
        private readonly IMenuManager menuManager;

        protected MenuViewModel(IMenuManager menuManager)
        {
            this.menuManager = menuManager ?? throw new System.ArgumentNullException(nameof(menuManager));
        }

        public IEnumerable<IMenuItem> Items
        {
            get
            {
                if(items is null)
                {
                    items = new ObservableCollection<IMenuItem>();
                    foreach (var item in Build())
                    {
                        items.Add(item);
                    }
                }

                return items;
            }
        }

        protected abstract IEnumerable<IMenuItem> Build();
    }
}