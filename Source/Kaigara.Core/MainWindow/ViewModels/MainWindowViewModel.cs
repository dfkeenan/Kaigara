using Kaigara.Commands;
using Kaigara.MainWindow.Views;
using Kaigara.Menus;
using Kaigara.Shell;
using Kaigara.Toolbars;
using Kaigara.ViewModels;

namespace Kaigara.MainWindow.ViewModels;

public class MainWindowViewModel : WindowViewModel
{
    public MenuViewModel MainMenu { get; }
    public ToolbarTrayViewModel ToolbarTray { get; }

    public IShell Shell { get; }
    public ICommandManager CommandManager { get; }
    public MainWindowView? View { get; internal set; }

    public MainWindowViewModel(IShell shell, ICommandManager commandManager, MainMenuViewModel mainMenu, MainToolbarTrayViewModel toolBarTray)
    {
        this.MainMenu = mainMenu ?? throw new ArgumentNullException(nameof(mainMenu));
        this.Shell = shell ?? throw new ArgumentNullException(nameof(shell));
        CommandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
        ToolbarTray = toolBarTray ?? throw new ArgumentNullException(nameof(toolBarTray));
    }

}
