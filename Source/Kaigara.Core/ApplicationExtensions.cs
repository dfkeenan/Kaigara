using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace Kaigara;

public static class ApplicationExtensions
{
    public static ShellAppBuilder ConfigureShellApp(this Application application,Func<ApplicationInfo, ApplicationInfo>? option = null)
    {
        if(application.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime)
            throw new ArgumentException($"Invalid ApplicationLifeTime. Only {nameof(IClassicDesktopStyleApplicationLifetime)} is supported",nameof(application));


        var appInfo = ApplicationInfo.FromAssembly(application.GetType().Assembly);

        appInfo = option?.Invoke(appInfo) ?? appInfo;

        return new ShellAppBuilder(application, appInfo);
    }
}
