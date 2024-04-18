using Autofac;
using Autofac.Core;
using Autofac.Extras.CommonServiceLocator;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommonServiceLocator;
using Kaigara.Configuration;
using Kaigara.MainWindow.Views;
using Kaigara.Shell;
using Kaigara.Themes;
using Kaigara.ViewModels;
using Microsoft.Extensions.Configuration;

namespace Kaigara;

public sealed class ShellAppBuilder : IShellAppBuilder
{
    private readonly Application application;
    private readonly string[] args;
    private ContainerBuilder containerBuilder;
    private ApplicationInfo appInfo;
    private IContainer? container;

    private Func<IShell, IConfiguration, IContainer, Startup>? startupFactory;

    public ShellAppBuilder(Application application, ApplicationInfo appInfo)
    {
        if (application?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime lifeTime)
            throw new ArgumentException($"Invalid ApplicationLifeTime. Only {nameof(IClassicDesktopStyleApplicationLifetime)} is supported", nameof(application));


        this.application = application ?? throw new ArgumentNullException(nameof(application));
        this.appInfo = appInfo;
        this.args = lifeTime.Args ?? [];
        containerBuilder = new ContainerBuilder();

        containerBuilder
            .RegisterInstance(application)
            .ExternallyOwned()
            .As<Application>()
            .SingleInstance();

        containerBuilder
            .RegisterType<ThemeManager>()
            .AsSelf()
            .SingleInstance();

        containerBuilder
            .RegisterInstance(appInfo)
            .ExternallyOwned()
            .AsSelf()
            .SingleInstance();
    }

    public IShellAppBuilder Configure(Action<ConfigurationBuilder> builderCallback)
    {
        if (builderCallback is null)
        {
            throw new ArgumentNullException(nameof(builderCallback));
        }

        containerBuilder.RegisterInstance(builderCallback).SingleInstance();

        return this;
    }

    public IShellAppBuilder AddDefaultConfiguration()
    {
        string userSettingsFilePath = Path.Combine(appInfo.ApplicationDataPath, "settings.json");

        if (!File.Exists(userSettingsFilePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(userSettingsFilePath)!);
        }

        if (!File.Exists(userSettingsFilePath))
            File.WriteAllText(userSettingsFilePath, $"{{{Environment.NewLine}}}");

        containerBuilder.RegisterModule(new ConfigurationModule { UserSettingFilePath = userSettingsFilePath, IncludeDefaultUI = true });

        return this;
    }

    public IShellAppBuilder Register(Action<ContainerBuilder> builderCallback)
    {
        if (builderCallback is null)
        {
            throw new ArgumentNullException(nameof(builderCallback));
        }

        builderCallback.Invoke(containerBuilder);

        return this;
    }

    public IShellAppBuilder RegisterModule<TModule>() where TModule : IModule, new()
    {
        containerBuilder.RegisterModule<TModule>();
        return this;
    }

    public IShellAppBuilder RegisterCoreModules()
    {
        containerBuilder.RegisterModule<CoreModule>();
        return this;
    }

    public IShellAppBuilder RegisterAllAppModels(NamespaceRule namespaceRule = NamespaceRule.StartsWith)
    {

        var appType = application.GetType();
        containerBuilder.RegisterAllModels(appType.Assembly, appType.Namespace!, namespaceRule);

        return this;
    }

    public IShellAppBuilder Startup(StartupDelegate startup)
    {
        startupFactory = (IShell shell, IConfiguration configuration, IContainer container)
            => new DelegatedStartup(shell, configuration, container, startup);

        return this;
    }

    public IShellAppBuilder Startup<TStartup>()
        where TStartup : Startup
    {
        startupFactory = (IShell shell, IConfiguration configuration, IContainer container)
            => (Startup)Activator.CreateInstance(typeof(TStartup), shell, configuration, container)!;

        return this;
    }

    public ShellApplication? Start(Action<IWindowViewModel>? mainWindowOptions = null)
    {
        if (application.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            container = containerBuilder.Build();

            var csl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => csl);

            startupFactory?.Invoke(container.Resolve<IShell>(), container.Resolve<IConfiguration>(), container)?.Start();

            var window = container.Resolve<MainWindowView>();
            mainWindowOptions?.Invoke(window.ViewModel!);

            lifetime.MainWindow = window;

            return new ShellApplication(container);
        }

        return null;
    }

    public ShellApplication? Start(string? title = null, Uri? iconUri = null, Size? size = null, WindowStartupLocation startLocation = WindowStartupLocation.CenterScreen)
    {
        title ??= appInfo.ProductName;
        iconUri ??= appInfo.IconUri;

        return Start(w =>
        {
            w.Title = title;
            w.IconUri = iconUri;
            w.Width = size?.Width ?? double.NaN;
            w.Height = size?.Height ?? double.NaN;
            w.StartupLocation = startLocation;
        });
    }
}

public record class ShellApplication(IContainer Container);
