using System.Collections.Specialized;
using System.Diagnostics;
using Autofac;
using Avalonia;
using Avalonia.Markup.Xaml;
using ExampleApplication.Documents.ViewModels;
using Kaigara;
using Kaigara.Menus;
using Kaigara.Reactive;
using Kaigara.Shell;
using Kaigara.Toolbars;
using Microsoft.Extensions.Configuration;
using ReactiveUI;


namespace ExampleApplication;

public partial class App : Application
{
    private ShellApplication? shellApplication;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        shellApplication = 
            this.ConfigureShellApp(info => info with
            {
                ProductName = "Example Application"
            })
            .AddDefaultConfiguration()
            //.Configure(c => c.AddJsonFile("custom.json", true, true))
            .RegisterModule<DefaultModule>()
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

        base.OnFrameworkInitializationCompleted();
    }
}
