using System;
using System.Reactive.Linq;
using Autofac;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ExampleApplication.Commands;
using ExampleApplication.Documents.ViewModels;
using Kaigara.Hosting;
using Kaigara.MainWindow.ViewModels;
using Kaigara.Menus;
using Kaigara.Shell;
using Microsoft.Extensions.Configuration;

namespace ExampleApplication
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) 
            => BuildAvaloniaApp()
                .ConfigureShellApp(args, info => info with
                {
                    ProductName = "Example Application"
                })
                .AddDefaultConfiguration()
                .RegisterDefaultModules()
                .RegisterAllAppModels()
                .Startup((IShell shell, IConfiguration configuration, IContainer container) =>
                {
                    var menuManager = container.Resolve<IMenuManager>();

                    MenuItemDefinition exampleDefinition = new MenuItemGroupDefinition("Example", "_Example")
                    {
                        new MenuItemDefinition("Thing1").BindCommand<ExampleCommand>(),
                        new MenuItemDefinition("Thing2", "Thing _2"),
                        new MenuItemDefinition("Thing3", "Thing _3"),
                    }.VisibleWhen<IShell>( s => s.DocumentActivated.Select(d => d is ExampleDocumentViewModel));

                    menuManager.Register(new MenuPath("MainMenu/File"), exampleDefinition);

                    menuManager.ConfigureMenuItemDefinition(new MenuPath("MainMenu/Edit"), definition =>
                    {
                        definition.VisibleWhen<IShell>(s => s.DocumentActivated.Select(d => d is ExampleDocumentViewModel));
                    });

                    shell.OpenDocument<ExampleDocumentViewModel>();
                    shell.OpenDocument<OtherDocumentViewModel>();
                })
                .Start(size: new Size(1920,1080));

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
    }
}
