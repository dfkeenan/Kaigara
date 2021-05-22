using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaigara.Menus;

namespace Kaigara.MainWindow.ViewModels
{
    public sealed class MainMenuViewModel : MenuViewModel
    {
        public MainMenuViewModel(IMenuManager menuManager) : base(menuManager)
        {
        }

        protected override IEnumerable<IMenuItem> Build()
            => new List<IMenuItem>
            {
                new MenuItemViewModel("File", "_File")
                {
                    new MenuItemViewModel("Exit", "E_xit")
                },
                new MenuItemViewModel("Edit", "_Edit")
                {
                    
                },
                new MenuItemViewModel("Window", "_Window")
                {
                   
                },
                new MenuItemViewModel("Help", "_Help")
                {
                    
                }
            };
    }
}
