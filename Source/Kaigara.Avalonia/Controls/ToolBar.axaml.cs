using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;

namespace Kaigara.Avalonia.Controls;

public class ToolBar : ItemsControl
{
    private static readonly FuncTemplate<IPanel> DefaultPanel =
        new FuncTemplate<IPanel>(() => new StackPanel { Orientation = Orientation.Horizontal });

    static ToolBar()
    {
        ItemsPanelProperty.OverrideDefaultValue<ToolBar>(DefaultPanel);
    }

    public ToolBar()
    {

    }
}
