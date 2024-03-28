using Autofac;
using Autofac.Core;
using Avalonia;
using Avalonia.Controls;
using Kaigara.ViewModels;
using Microsoft.Extensions.Configuration;

namespace Kaigara;

internal sealed class DesignTimeShellAppBuilder : IShellAppBuilder
{
    private Application application;
    private ApplicationInfo appInfo;

    public DesignTimeShellAppBuilder(Application application, ApplicationInfo appInfo)
    {
        this.application = application;
        this.appInfo = appInfo;
    }

    public IShellAppBuilder AddDefaultConfiguration()
        => this;

    public IShellAppBuilder Configure(Action<ConfigurationBuilder> builderCallback)
        => this;

    public IShellAppBuilder Register(Action<ContainerBuilder> builderCallback)
        => this;

    public IShellAppBuilder RegisterAllAppModels(NamespaceRule namespaceRule = NamespaceRule.StartsWith)
        => this;

    public IShellAppBuilder RegisterCoreModules()
        => this;

    public IShellAppBuilder RegisterModule<TModule>() 
        where TModule : IModule, new()
        => this;

    public ShellApplication? Start(Action<IWindowViewModel>? mainWindowOptions = null)
        => null;

    public ShellApplication? Start(string? title = null, Uri? iconUri = null, Size? size = null, WindowStartupLocation startLocation = WindowStartupLocation.CenterScreen)
        => null;

    public IShellAppBuilder Startup(StartupDelegate startup)
        => this;

    public IShellAppBuilder Startup<TStartup>() 
        where TStartup : Startup
        => this;
}