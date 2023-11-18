using Kaigara.Menus;

namespace Kaigara.MainWindow.ViewModels;

public class MainMenuViewModel : MenuViewModel
{
    public MainMenuViewModel()
        : base(GetDefinition())
    {

    }

    private static MenuDefinition GetDefinition()
    {
        int order = 0;

        return new MenuDefinition("MainMenu")
        {
                new MenuItemDefinition("File", "_File", displayOrder: order += 100)
                {
                    new MenuItemGroupDefinition("ExitGroup", displayOrder: int.MaxValue)
                    {
                    }
                },
                //new MenuItemDefinition("Edit", "_Edit", displayOrder: order += 100)
                //{

                //},
                //new MenuItemDefinition("Window", "_Window", displayOrder: order += 100)
                //{

                //},
                //new MenuItemDefinition("Help", "_Help", displayOrder : order += 100)
                //{

                //}
        };
    }
}
