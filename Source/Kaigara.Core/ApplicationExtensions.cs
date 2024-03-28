using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Kaigara;

public static class ApplicationExtensions
{
    public static IShellAppBuilder ConfigureShellApp(this Application application,Func<ApplicationInfo, ApplicationInfo>? option = null)
    {
        var appInfo = ApplicationInfo.FromAssembly(application.GetType().Assembly);

        appInfo = option?.Invoke(appInfo) ?? appInfo;

        if(Design.IsDesignMode)
        {
            return new DesignTimeShellAppBuilder(application, appInfo);
        }

        if (application.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime)
            throw new ArgumentException($"Invalid ApplicationLifeTime. Only {nameof(IClassicDesktopStyleApplicationLifetime)} is supported", nameof(application));

        return new ShellAppBuilder(application, appInfo);
    }
}
