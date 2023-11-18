using Autofac;
using Autofac.Features.AttributeFilters;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Kaigara.Commands;
using Kaigara.Dialogs;
using Kaigara.MainWindow.ViewModels;
using Kaigara.MainWindow.Views;
using Kaigara.Menus;
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
            .RegisterCommands<MainWindowModule>();


        int order = 0;

        builder.RegisterMenu(
             new MenuDefinition("MainMenu")
            {
                    new MenuItemDefinition("File", "_File", displayOrder: order += 100)
                    {
                        new MenuItemGroupDefinition("ExitGroup", displayOrder: int.MaxValue)
                        {
                        }
                    },
            });

        builder.RegisterToolbarTray(new ToolbarTrayDefinition("MainToolbarTray")
        {

        });


        builder.RegisterType<MainWindowViewModel>()
        .SingleInstance()
        .AsSelf()
        .WithAttributeFiltering();

        builder.Register((MainWindowViewModel vm) => 
        {
            var result = new MainWindowView()
            {
                DataContext = vm
            };
            return result;
        })
        .SingleInstance()
        .ExternallyOwned();
    }

}
