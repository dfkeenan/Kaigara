using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Kaigara.Commands;
using Kaigara.Menus;
using Kaigara.Toolbars;

namespace Kaigara.MainWindow.Commands;

[MenuItemDefinition("Exit", "MainMenu/File/ExitGroup", Label = "E_xit")]
[CommandDefinition("Example Command", DefaultInputGesture = "Alt+F4")]
public class ExitApplicationCommand : RegisteredCommand
{
    protected override void OnExecute()
    {
        if(Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            lifetime.MainWindow?.Close();
        }
    }
}
