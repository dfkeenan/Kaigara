using Kaigara.ToolBars;

namespace Kaigara.MainWindow.ViewModels;

public class MainToolBarTrayViewModel : ToolBarTrayViewModel
{
    public MainToolBarTrayViewModel()
        : base(new ToolBarTrayDefinition("MainToolBarTray")
        {

        })
    {
    }
}
