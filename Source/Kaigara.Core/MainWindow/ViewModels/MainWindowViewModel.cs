using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaigara.Commands;
using Kaigara.Menus;
using Kaigara.Shell;
using Kaigara.ToolBars;
using Kaigara.ViewModels;
using ReactiveUI;

namespace Kaigara.MainWindow.ViewModels
{
    public class MainWindowViewModel : WindowViewModel
    {
        public MenuViewModel MainMenu { get; }
        public ToolBarTrayViewModel ToolBarTray { get; }

        public IShell Shell { get; }
        public ICommandManager CommandManager { get; }

        public MainWindowViewModel(IShell shell, ICommandManager commandManager, MainMenuViewModel mainMenu, MainToolBarTrayViewModel toolBarTray)
        {
            this.MainMenu = mainMenu ?? throw new ArgumentNullException(nameof(mainMenu));
            this.Shell = shell ?? throw new ArgumentNullException(nameof(shell));
            CommandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
            ToolBarTray = toolBarTray ?? throw new ArgumentNullException(nameof(toolBarTray));
        }

    }
}
