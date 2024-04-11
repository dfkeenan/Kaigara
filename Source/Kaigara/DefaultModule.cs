using Autofac;
using Kaigara.About;
using Kaigara.About.Commands;
using Kaigara.Menus;

namespace Kaigara;
public class DefaultModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.DependsOnModule<CoreModule>();
        builder.DependsOnModule<AboutModule>();
        builder.RegisterAllModels<DefaultModule>();


        builder.RegisterBuildCallback(c =>
        {
            var menuManager = c.Resolve<IMenuManager>();
            int order = 100;
            menuManager.Register(new MenuItemLocation("MainMenu"),
                new MenuItemDefinition("Edit", "_Edit", displayOrder: order += 100)
                {

                },
                new MenuItemDefinition("Window", "_Window", displayOrder: order += 100)
                {

                },
                new MenuItemDefinition("Help", "_Help", displayOrder: order += 100)
                {
                    menuManager.CreateMenuItemDefinition<ShowAboutDialogCommand>("About", displayOrder: int.MaxValue)
                }
                );
        });
    }
}
