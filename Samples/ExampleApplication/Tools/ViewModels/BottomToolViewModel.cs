using Dock.Model.ReactiveUI.Controls;
using Kaigara.Shell;

namespace ExampleApplication.Tools.ViewModels;

[Tool(ShellDockIds.BottomToolDock)]
public class BottomToolViewModel : Tool
{
    public BottomToolViewModel()
    {
        Id = Guid.NewGuid().ToString();
        Title = "Bottom Tool";
    }
}
