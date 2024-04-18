using Avalonia;
using Avalonia.Styling;
using Avalonia.Threading;

namespace Kaigara.Themes;
public class ThemeManager
{
    private readonly Application application;

    public ThemeManager(Application application)
    {
        this.application = application ?? throw new ArgumentNullException(nameof(application));

        //application.RequestedThemeVariant
        //TODO: Find other themes
        Themes = [ThemeVariant.Light, ThemeVariant.Dark];
    }

    public IEnumerable<ThemeVariant> Themes { get; }
    public ThemeVariant CurrentTheme => application.ActualThemeVariant;

    public void ChangeTheme(string? themeName)
    {
        if (themeName == null) return;

        if (Themes.FirstOrDefault(t => t.Key.ToString() == themeName) is ThemeVariant theme)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                application.RequestedThemeVariant = theme;
            });
        }
    }
}
