using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;

namespace Kaigara.Avalonia.Controls;

public class Toolbar : ItemsControl
{
    private static readonly FuncTemplate<Panel?> DefaultPanel =
        new(() => new StackPanel { Orientation = Orientation.Horizontal });

    static Toolbar()
    {
        ItemsPanelProperty.OverrideDefaultValue<Toolbar>(DefaultPanel);
    }

    public Toolbar()
    {

    }
}
