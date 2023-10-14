using Autofac;
using Autofac.Core;
using Autofac.Extras.CommonServiceLocator;
using Avalonia;
using Avalonia.Controls;
using CommonServiceLocator;
using Kaigara.MainWindow.ViewModels;
using Kaigara.MainWindow.Views;
using Kaigara.Shell;
using Kaigara.ViewModels;
using Microsoft.Extensions.Configuration;

namespace Kaigara.Hosting;

public sealed class ShellAppBuilder
{
    private readonly AppBuilder appBuilder;
    private readonly string[] args;
    private ContainerBuilder containerBuilder;
    private ConfigurationBuilder configurationBuilder;
    private ApplicationInfo appInfo;
    private IContainer? container;

    private Func<IShell, IConfiguration, IContainer, Startup>? startupFactory;

    public ShellAppBuilder(AppBuilder appBuilder, ApplicationInfo appInfo, string[] args)
    {
        this.appBuilder = appBuilder ?? throw new ArgumentNullException(nameof(appBuilder));
        this.appInfo = appInfo;
        this.args = args;
        containerBuilder = new ContainerBuilder();
        configurationBuilder = new ConfigurationBuilder();
    }

    public ShellAppBuilder Configure(Action<ConfigurationBuilder> builderCallback)
    {
        if (builderCallback is null)
        {
            throw new ArgumentNullException(nameof(builderCallback));
        }

        builderCallback.Invoke(configurationBuilder);

        return this;
    }

    public ShellAppBuilder AddDefaultConfiguration()
    {
        string userSettingsFilePath = Path.Combine(appInfo.ApplicationDataPath, "settings.json");

        if (!File.Exists(userSettingsFilePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(userSettingsFilePath)!);
        }

        configurationBuilder.SetBasePath(Environment.CurrentDirectory)
                            .AddJsonFile("appsettings.json", true)
                            .AddJsonFile(userSettingsFilePath, true);

        return this;
    }

    public ShellAppBuilder Register(Action<ContainerBuilder> builderCallback)
    {
        if (builderCallback is null)
        {
            throw new ArgumentNullException(nameof(builderCallback));
        }

        builderCallback.Invoke(containerBuilder);

        return this;
    }

    public ShellAppBuilder RegisterModule<TModule>() where TModule : IModule, new()
    {
        containerBuilder.RegisterModule<TModule>();
        return this;
    }

    public ShellAppBuilder RegisterDefaultModules()
    {
        containerBuilder.RegisterModule<DefaultsModule>();
        return this;
    }

    public ShellAppBuilder RegisterAllAppModels(NamespaceRule namespaceRule = NamespaceRule.StartsWith)
    {
        var appType = this.appBuilder.ApplicationType;
        containerBuilder.RegisterAllModels(appType.Assembly, appType.Namespace!, namespaceRule);

        return this;
    }

    public ShellAppBuilder Startup(StartupDelegate startup)
    {
        startupFactory = (IShell shell, IConfiguration configuration, IContainer container)
            => new DelegatedStartup(shell, configuration, container, startup);

        return this;
    }

    public ShellAppBuilder Startup<TStartup>()
        where TStartup : Startup
    {
        startupFactory = (IShell shell, IConfiguration configuration, IContainer container)
            => (Startup)Activator.CreateInstance(typeof(TStartup), shell, configuration, container)!;

        return this;
    }

    public void Start(Action<IWindowViewModel>? mainWindowOptions = null)
    {
        var configuration = configurationBuilder.Build();
        containerBuilder.RegisterInstance(configuration).As<IConfiguration>();

        container = containerBuilder.Build();

        var csl = new AutofacServiceLocator(container);
        ServiceLocator.SetLocatorProvider(() => csl);

        startupFactory?.Invoke(container.Resolve<IShell>(), container.Resolve<IConfiguration>(), container)?.Start();

        appBuilder.Start(StartApplication, args);

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
        title ??= appInfo.ProductName;
        iconUri ??= appInfo.IconUri;

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
