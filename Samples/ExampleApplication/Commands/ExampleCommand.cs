using System.Diagnostics;
using ExampleApplication.Tools.ViewModels;
using Kaigara.Commands;
using Kaigara.Menus;
using Kaigara.Shell;
using Kaigara.ToolBars;

namespace ExampleApplication.Commands;

[MenuItemDefinition("Example1", "MainMenu/Example")]
[ToolBarItemDefinition("Example1", "MainToolBarTray/Example")]
[CommandDefinition("Example Command", DefaultInputGesture = "Ctrl+E", IconName = "ExampleIcon")]
public class ExampleCommand : RegisteredCommand
{
    private readonly IShell shell;

    public ExampleCommand(IShell shell)
    {
        this.shell = shell;
    }
    protected override void OnExecute()
    {
        Debug.WriteLine($"Hello {Label} - {Name}, {InputGesture}");
        shell.Tools.Open<IRightToolViewModel>();
        shell.Tools.Open<LeftToolViewModel>();
        shell.Tools.Open(new BottomToolViewModel());
    }
}
