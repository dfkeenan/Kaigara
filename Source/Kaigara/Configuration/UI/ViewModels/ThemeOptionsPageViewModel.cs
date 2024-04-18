using Kaigara.Themes;
using ReactiveUI;

namespace Kaigara.Configuration.UI.ViewModels;

[OptionsPage<ThemeOptions, EnvironmentCategory>("Theme", DisplayOrder = -1000)]
public class ThemeOptionsPageViewModel : OptionsPageViewModel
{
    private readonly ThemeManager themeManager;
    private string? themeName;

    public ThemeOptionsPageViewModel(ThemeManager themeManager)
    {
        this.themeManager = themeManager ?? throw new ArgumentNullException(nameof(themeManager));

        ThemeName = themeManager.CurrentTheme.Key.ToString();
    }

    public string? ThemeName
    {
        get => themeName;
        set => this.RaiseAndSetIfChanged(ref themeName, value);
    }

    public IEnumerable<string> Themes => themeManager.Themes.Select(t => t.Key.ToString()!);

}
