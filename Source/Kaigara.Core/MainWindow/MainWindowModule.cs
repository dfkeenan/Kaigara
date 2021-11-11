using Autofac;
using Kaigara.Commands;
using Kaigara.Menus;
using Kaigara.Shell;
using Kaigara.ToolBars;

namespace Kaigara.MainWindow;

public class MainWindowModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder
            .DependsOnModule<MenuModule>()
            .DependsOnModule<ToolBarModule>()
            .DependsOnModule<ShellModule>()
            .DependsOnModule<CommandModule>()
            .RegisterViewModels<MainWindowModule>()
            .RegisterMenus<MainWindowModule>()
            .RegisterToolBars<MainWindowModule>();


        //builder.Register(c=> 
        //{
        //    var menu = new MainMenuViewModel();
        //    c.Resolve<IMenuManager>().Register(menu.Definition); 
        //    return menu; 
        //}).AsSelf().SingleInstance();
    }

}
