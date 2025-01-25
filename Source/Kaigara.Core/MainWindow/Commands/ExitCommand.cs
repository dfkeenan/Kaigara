using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Kaigara.Commands;
using Kaigara.Menus;

namespace Kaigara.MainWindow.Commands;

[MenuItemDefinition("Exit", "MainMenu/File/ExitGroup", Label = "E_xit")]
[CommandDefinition("Example Command", DefaultInputGesture = "Alt+F4")]
public class ExitApplicationCommand : RegisteredCommand
{
    protected override void OnExecute()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            lifetime.MainWindow?.Close();
        }
    }
}
