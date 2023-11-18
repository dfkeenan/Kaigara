﻿using Autofac.Features.AttributeFilters;
using Kaigara.Commands;
using Kaigara.MainWindow.Views;
using Kaigara.Menus;
using Kaigara.Shell;
using Kaigara.Toolbars;
using Kaigara.ViewModels;

namespace Kaigara.MainWindow.ViewModels;

public class MainWindowViewModel(IShell shell, ICommandManager commandManager, [KeyFilter("MainMenu")]MenuViewModel mainMenu, MainToolbarTrayViewModel toolBarTray) : WindowViewModel
{
    public MenuViewModel MainMenu { get; } = mainMenu ?? throw new ArgumentNullException(nameof(mainMenu));
    public ToolbarTrayViewModel ToolbarTray { get; } = toolBarTray ?? throw new ArgumentNullException(nameof(toolBarTray));

    public IShell Shell { get; } = shell ?? throw new ArgumentNullException(nameof(shell));
    public ICommandManager CommandManager { get; } = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
    public MainWindowView? View { get; internal set; }
}
