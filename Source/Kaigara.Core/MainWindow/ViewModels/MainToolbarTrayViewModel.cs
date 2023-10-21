using Kaigara.Toolbars;

namespace Kaigara.MainWindow.ViewModels;

public class MainToolbarTrayViewModel : ToolbarTrayViewModel
{
    public MainToolbarTrayViewModel()
        : base(new ToolbarTrayDefinition("MainToolbarTray")
        {

        })
    {
    }
}
