using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Templates;
using Avalonia.Layout;

namespace Kaigara.Avalonia.Controls;

public class ToolBarTray : ItemsControl
{
    private static readonly FuncTemplate<Panel?> DefaultPanel =
        new(() => new WrapPanel { Orientation = Orientation.Horizontal });

    static ToolBarTray()
    {
        ItemsPanelProperty.OverrideDefaultValue<ToolBarTray>(DefaultPanel);
    }

    public ToolBarTray()
    {

    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new ToolBar();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<ToolBar>(item, out recycleKey);
    }
}
