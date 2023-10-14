using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;

namespace Kaigara.Avalonia.Controls;

public class ToolBar : ItemsControl
{
    private static readonly FuncTemplate<Panel?> DefaultPanel =
        new(() => new StackPanel { Orientation = Orientation.Horizontal });

    static ToolBar()
    {
        ItemsPanelProperty.OverrideDefaultValue<ToolBar>(DefaultPanel);
    }

    public ToolBar()
    {

    }
}
