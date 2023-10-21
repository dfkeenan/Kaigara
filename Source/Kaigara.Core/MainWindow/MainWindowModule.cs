using Autofac;
using Avalonia.Platform.Storage;
using Kaigara.Commands;
using Kaigara.Dialogs;
using Kaigara.MainWindow.ViewModels;
using Kaigara.MainWindow.Views;
using Kaigara.Menus;
using Kaigara.Services;
using Kaigara.Shell;
using Kaigara.Toolbars;

namespace Kaigara.MainWindow;

public class MainWindowModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder
            .DependsOnModule<MenuModule>()
            .DependsOnModule<ToolbarModule>()
            .DependsOnModule<ShellModule>()
            .DependsOnModule<CommandModule>()
            .DependsOnModule<DialogsModule>()
            .RegisterMenus<MainWindowModule>()
            .RegisterToolbars<MainWindowModule>();

        builder.RegisterType<MainWindowViewModel>()
        .SingleInstance().AsSelf();

        builder.RegisterType<StorageProviderDecorator<MainWindowView>>()
            .As<IStorageProvider>()
            .SingleInstance();
    }

}
