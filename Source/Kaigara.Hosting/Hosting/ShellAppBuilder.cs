using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia;
using Autofac;
using Autofac.Core;
using Kaigara.Views;
using Avalonia.Platform;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Kaigara.ViewModels;
using System.Reflection;

namespace Kaigara.Hosting
{
    public static class AppBuilderExtensions
    {
        public static ShellAppBuilder<TAppBuilder> ConfigureShellApp<TAppBuilder>(this AppBuilderBase<TAppBuilder> appBuilder, string[] args)
             where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
        {
            return new ShellAppBuilder<TAppBuilder>(appBuilder, args);
        }
    }


    public sealed class ShellAppBuilder<TAppBuilder>
            where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
    {
        private readonly AppBuilderBase<TAppBuilder> appBuilder;
        private readonly string[] args;
        private ContainerBuilder containerBuilder;
        private IContainer? container;

        public ShellAppBuilder(AppBuilderBase<TAppBuilder> appBuilder, string[] args)
        {
            this.appBuilder = appBuilder ?? throw new ArgumentNullException(nameof(appBuilder));
            this.args = args;
            containerBuilder = new ContainerBuilder();
        }

        public ShellAppBuilder<TAppBuilder> RegisterModule<TModule>() where TModule : IModule, new()
        {
            containerBuilder.RegisterModule<TModule>();
            return this;
        }

        public ShellAppBuilder<TAppBuilder> RegisterDefaultModules()
        {
            containerBuilder.RegisterModule<DefaultsModule>();
            return this;
        }

        public void Start(Action<IWindowViewModel>? mainWindowOptions = null)
        {
            container = containerBuilder.Build();

            var csl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => csl);

            
            appBuilder.Start(StartApplication,args);

            void StartApplication(Application app, string[] args)
            {
                var mainWindowViewModel = container.Resolve<MainWindowViewModel>();
                mainWindowOptions?.Invoke(mainWindowViewModel);

                var window = new MainWindowView()
                {
                    DataContext = mainWindowViewModel,
                };

                app.Run(window);
            }
        }

        public void Start(string? title = null, Uri? iconUri = null, Size? size = null, WindowStartupLocation startLocation = WindowStartupLocation.CenterScreen)
        {
            var entryAssembly = Assembly.GetEntryAssembly()!;

            title ??= entryAssembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ??
                      entryAssembly.GetName().Name;

            iconUri ??= new Uri($"resm:{entryAssembly.GetName().Name}.Application.ico");

            Start(w =>
            {
                w.Title = title;
                w.IconUri = iconUri;
                w.Width = size?.Width ?? double.NaN;
                w.Height = size?.Height ?? double.NaN;
                w.StartupLocation = startLocation;
            });
        }
    }
}
