using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Kaigara.Avalonia.Views;

namespace Kaigara.Shell;
public class ShellViewLocator : ViewLocator
{
    private static ConditionalWeakTable<object,Control> controlTable = new();

    protected override void ViewLocated(object data, Control control)
    {
        if(data is not IDockable) return;

        controlTable.AddOrUpdate(data, control);
    }

    public static Control? GetControl(object data)
        => controlTable.TryGetValue(data, out var control) ? control : null;

    public static TopLevel? GetTopLevel(object data)
        => controlTable.TryGetValue(data, out var control) ? TopLevel.GetTopLevel(control) : null;

    internal static void RegisterShell<T>(T shell, Control control)
        where T : class, IShell
    {
        controlTable.AddOrUpdate(shell, control);
    }
}
