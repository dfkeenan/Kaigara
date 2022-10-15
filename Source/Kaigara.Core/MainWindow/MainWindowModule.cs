using Autofac;
using Kaigara.Commands;
using Kaigara.Dialogs;
using Kaigara.MainWindow.ViewModels;
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
            .DependsOnModule<DialogsModule>()
            .RegisterMenus<MainWindowModule>()
            .RegisterToolBars<MainWindowModule>();

        builder.RegisterType<MainWindowViewModel>()
        .SingleInstance().AsSelf();
    }

}
