using System.Diagnostics;
using ExampleApplication.Tools.ViewModels;
using Kaigara.Commands;
using Kaigara.Menus;
using Kaigara.Shell;
using Kaigara.Toolbars;

namespace ExampleApplication.Commands;

[MenuItemDefinition("Example1", "MainMenu/File/Example")]
[ToolbarItemDefinition("Example1", "MainToolbarTray/Example")]
[CommandDefinition("Example Command", DefaultInputGesture = "Ctrl+E", IconName = "Run")]
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
