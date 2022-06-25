using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Primitives;

namespace Kaigara.Avalonia.Controls;
public class InspectorItem : TreeViewItem
{
    protected override IItemContainerGenerator CreateItemContainerGenerator()
        => CreateTreeItemContainerGenerator<InspectorItem>();
}
