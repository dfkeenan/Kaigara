using Autofac;
using Kaigara.Themes;
using Microsoft.Extensions.Options;

namespace Kaigara.Configuration;
public class ThemeOptionsWatcher : IDisposable, IStartable
{
    private readonly ThemeManager themeManager;
    private readonly IOptionsMonitor<ThemeOptions> optionsMonitor;
    private IDisposable? monitor;

    public ThemeOptionsWatcher(ThemeManager themeManager, IOptionsMonitor<ThemeOptions> optionsMonitor)
    {
        this.themeManager = themeManager;
        this.optionsMonitor = optionsMonitor;

        themeManager.ChangeTheme(optionsMonitor.CurrentValue.ThemeName);
    }

    public void Start()
    {
        monitor = optionsMonitor.OnChange((theme) =>
        {
            themeManager.ChangeTheme(theme.ThemeName);
        });
    }

    public void Dispose()
    {
        monitor?.Dispose();
    }
}
