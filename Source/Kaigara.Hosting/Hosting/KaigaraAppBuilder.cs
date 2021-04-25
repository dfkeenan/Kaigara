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

namespace Kaigara.Hosting
{
    public static class AppBuilderExtensions
    {
        public static KaigaraAppBuilder<TAppBuilder> ConfigureKaigara<TAppBuilder>(this AppBuilderBase<TAppBuilder> appBuilder, string[] args)
             where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
        {
            return new KaigaraAppBuilder<TAppBuilder>(appBuilder, args);
        }
    }


    public sealed class KaigaraAppBuilder<TAppBuilder>
            where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
    {
        private readonly AppBuilderBase<TAppBuilder> appBuilder;
        private readonly string[] args;
        private Uri? windowIconUri;
        private ContainerBuilder containerBuilder;
        private IContainer? container;

        public KaigaraAppBuilder(AppBuilderBase<TAppBuilder> appBuilder, string[] args)
        {
            this.appBuilder = appBuilder ?? throw new ArgumentNullException(nameof(appBuilder));
            this.args = args;
            containerBuilder = new ContainerBuilder();
        }

        public KaigaraAppBuilder<TAppBuilder> RegisterModule<TModule>() where TModule : IModule, new()
        {
            containerBuilder.RegisterModule<TModule>();
            return this;
        }

        public KaigaraAppBuilder<TAppBuilder> RegisterDefaultModules()
        {
            
            return this;
        }

        public void Start()
        {
            container = containerBuilder.Build();

            var csl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => csl);

            
            appBuilder.Start(StartApplication,args);

            void StartApplication(Application app, string[] args)
            {
                WindowIcon? icon = null;

                //IAssetLoader assetLoader = AvaloniaLocator.Current.GetService<IAssetLoader>();
                //if (assetLoader.Exists(new Uri("")))
                //{
                //    icon = new WindowIcon(assetLoader.Open(new Uri("")));
                //}

                var window = new MainWindow()
                {
                    Icon = icon,
                    ViewModel = new ViewModels.MainWindowViewModel(),
                };

                app.Run(window);
            }
        }
    }
}
