using Kaigara.Menus;

namespace Kaigara.MainWindow.ViewModels;

public class MainMenuViewModel : MenuViewModel
{
    public MainMenuViewModel()
        : base(new MenuDefinition("MainMenu")
        {
                new MenuItemDefinition("File", "_File")
                {
                    new MenuItemDefinition("Exit", "E_xit")
                },
                new MenuItemDefinition("Edit", "_Edit")
                {

                },
                new MenuItemDefinition("Window", "_Window")
                {

                },
                new MenuItemDefinition("Help", "_Help")
                {

                }
        })
    {

    }
}
