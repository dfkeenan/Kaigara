using System;
using Autofac;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ExampleApplication.Documents.ViewModels;
using Kaigara.Hosting;
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
                    shell.OpenDocument<ExampleDocumentViewModel>();
                })
                .Start(size: new Size(1920,1080));

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
    }
}
