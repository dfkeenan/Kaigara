using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;

namespace Kaigara.Avalonia.Controls;

public class ToolbarTray : ItemsControl
{
    private static readonly FuncTemplate<Panel?> DefaultPanel =
        new(() => new WrapPanel { Orientation = Orientation.Horizontal });

    static ToolbarTray()
    {
        ItemsPanelProperty.OverrideDefaultValue<ToolbarTray>(DefaultPanel);
    }

    public ToolbarTray()
    {

    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new Toolbar();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<Toolbar>(item, out recycleKey);
    }
}
