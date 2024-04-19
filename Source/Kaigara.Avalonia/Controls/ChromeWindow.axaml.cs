using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Platform;

namespace Kaigara.Avalonia.Controls;

[PseudoClasses(":platformWindows", ":platformOSX", ":platformLinux")]
public class ChromeWindow : Window
{
    public static readonly StyledProperty<IDataTemplate?> TitleBarTemplateProperty = AvaloniaProperty.Register<ChromeWindow, IDataTemplate?>(nameof(TitleBarTemplate));

    public IDataTemplate? TitleBarTemplate
    {
        get => GetValue(TitleBarTemplateProperty);
        set => SetValue(TitleBarTemplateProperty, value);
    }

    public static readonly StyledProperty<double> IconSizeProperty = AvaloniaProperty.Register<ChromeWindow, double>(nameof(IconSize), 16);

    public double IconSize
    {
        get => GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    public ChromeWindow()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            PseudoClasses.Add(":platformWindows");
            if (Environment.OSVersion.Version.Build >= 22000)
            {
                //HACK: Win 11 has rounded corners.
                CornerRadius = new CornerRadius(8);
            }
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            PseudoClasses.Add(":platformOSX");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            PseudoClasses.Add(":platformLinux");
        }
    }

    public ChromeWindow(IWindowImpl impl)
        : base(impl)
    {

    }

    protected override Type StyleKeyOverride => typeof(ChromeWindow);
}
