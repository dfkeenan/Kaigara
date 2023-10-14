using Avalonia;
using Avalonia.Controls;

namespace Kaigara.Hosting;

public static class AppBuilderExtensions
{
    public static ShellAppBuilder ConfigureShellApp(this AppBuilder appBuilder, string[] args, Func<ApplicationInfo, ApplicationInfo>? option = null)
    {
        var appInfo = ApplicationInfo.FromEntryAssembly();

        appInfo = option?.Invoke(appInfo) ?? appInfo;

        return new ShellAppBuilder(appBuilder, appInfo, args);
    }
}
