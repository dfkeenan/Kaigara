using Avalonia.Controls;

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
}
