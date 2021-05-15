using System;
using Avalonia.Controls;

namespace Kaigara.Hosting
{
    public static class AppBuilderExtensions
    {
        public static ShellAppBuilder<TAppBuilder> ConfigureShellApp<TAppBuilder>(this AppBuilderBase<TAppBuilder> appBuilder, string[] args, Func<ApplicationInfo, ApplicationInfo>? option = null)
             where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
        {
            var appInfo = ApplicationInfo.FromEntryAssembly();

            appInfo = option?.Invoke(appInfo) ?? appInfo;

            return new ShellAppBuilder<TAppBuilder>(appBuilder, appInfo, args);
        }
    }
}
