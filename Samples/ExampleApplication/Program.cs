using System.Collections.Specialized;
using System.Diagnostics;
using Autofac;
using Avalonia;
using Avalonia.ReactiveUI;
using ExampleApplication.Documents.ViewModels;
using Kaigara.Hosting;
using Kaigara.Menus;
using Kaigara.Reactive;
using Kaigara.Shell;
using Kaigara.Toolbars;
using Microsoft.Extensions.Configuration;
using ReactiveUI;

namespace ExampleApplication;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
        => BuildAvaloniaApp()
            .ConfigureShellApp(args, info => info with
            {
                ProductName = "Example Application"
            })
            .AddDefaultConfiguration()
            //.Configure(c => c.AddJsonFile("custom.json", true, true))
            .RegisterDefaultModules()
            .RegisterAllAppModels()
            .Startup((IShell shell, IConfiguration configuration, IContainer container) =>
            {
                (shell.Dockables as INotifyCollectionChanged).CollectionChanged += (s, e) =>
                {
                    Debug.WriteLine("***************************");
                    Debug.WriteLine($"Dockables {shell.Dockables.Count}");
                    Debug.WriteLine($"Tools {shell.Tools.Count}");
                    Debug.WriteLine($"Documents {shell.Documents.Count}");
                };

                var menuManager = container.Resolve<IMenuManager>();

                MenuItemDefinition exampleDefinition = new MenuItemGroupDefinition("Example")
                {
                        new MenuItemDefinition("Thing2", "Thing _2"),
                        new MenuItemDefinition("Thing3", "Thing _3"),
                }.VisibleWhen<IShell>(s => s.Documents.Active.Is<ExampleDocumentViewModel>());


                menuManager.Register(new MenuItemLocation("/File"), exampleDefinition);

                menuManager.ConfigureDefinition(new MenuItemLocation("MainMenu/Edit"), definition =>
                {
                    definition.VisibleWhen<IShell>(s => s.Dockables.Active.Is<ExampleDocumentViewModel>(e => e.WhenAnyValue(e => e.IsChecked)));
                });

                var toolBarManager = container.Resolve<IToolbarManager>();

                var exampleToolbar = new ToolbarDefinition("Example")
                {
                    //new ToolbarItemDefinition("FIrst").BindCommand<ExampleCommand>(),
                }.VisibleWhen<IShell>(s => s.Documents.Active.Is<ExampleDocumentViewModel>());

                toolBarManager.Register(new ToolbarLocation("MainToolbarTray"), exampleToolbar);

                shell.Documents.Open<ExampleDocumentViewModel>();
                shell.Documents.Open<OtherDocumentViewModel>();


            })
            .Start(size: new Size(1920, 1080));

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseReactiveUI()
            .LogToTrace();
}
