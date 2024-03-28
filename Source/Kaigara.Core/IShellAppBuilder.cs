using Autofac;
using Autofac.Core;
using Avalonia;
using Avalonia.Controls;
using Kaigara.ViewModels;
using Microsoft.Extensions.Configuration;

namespace Kaigara;
public interface IShellAppBuilder
{
    IShellAppBuilder AddDefaultConfiguration();
    IShellAppBuilder Configure(Action<ConfigurationBuilder> builderCallback);
    IShellAppBuilder Register(Action<ContainerBuilder> builderCallback);
    IShellAppBuilder RegisterAllAppModels(NamespaceRule namespaceRule = NamespaceRule.StartsWith);
    IShellAppBuilder RegisterCoreModules();
    IShellAppBuilder RegisterModule<TModule>() where TModule : IModule, new();
    ShellApplication? Start(Action<IWindowViewModel>? mainWindowOptions = null);
    ShellApplication? Start(string? title = null, Uri? iconUri = null, Size? size = null, WindowStartupLocation startLocation = WindowStartupLocation.CenterScreen);
    IShellAppBuilder Startup(StartupDelegate startup);
    IShellAppBuilder Startup<TStartup>() where TStartup : Startup;
}