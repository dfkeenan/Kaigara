using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaigara.Menus;
using Kaigara.Shell;
using ReactiveUI;

namespace Kaigara.ViewModels
{
    public class MainWindowViewModel : WindowViewModel
    {
        private Menu mainMenu;
        private readonly IMenuManager menuManager;
        private readonly IShell shell;

        public Menu MainMenu
        {
            get { return mainMenu; }
            set { this.RaiseAndSetIfChanged(ref mainMenu, value); }
        }

        public IShell Shell => shell;

        public MainWindowViewModel(IMenuManager menuManager, IShell shell)
        {
            this.menuManager = menuManager ?? throw new ArgumentNullException(nameof(menuManager));
            this.shell = shell ?? throw new ArgumentNullException(nameof(shell));
        }

    }
}
