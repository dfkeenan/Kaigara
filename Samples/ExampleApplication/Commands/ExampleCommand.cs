using System.Diagnostics;
using ExampleApplication.Documents.ViewModels;
using ExampleApplication.Tools.ViewModels;
using Kaigara.Commands;
using Kaigara.Menus;
using Kaigara.Shell;
using Kaigara.Shell.Commands;
using Kaigara.Toolbars;

namespace ExampleApplication.Commands;

[MenuItemDefinition("Example1", "MainMenu/File/Example")]
[ToolbarItemDefinition("Example1", "MainToolbarTray/Example", CanExecuteBehavior = CanExecuteBehavior.Visible)]
[CommandDefinition("Example Command", DefaultInputGesture = "Ctrl+E", IconName = "Run")]
public class ExampleCommand : ActiveDocumentCommand<ExampleDocumentViewModel>
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
