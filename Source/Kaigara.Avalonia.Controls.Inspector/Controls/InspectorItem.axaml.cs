using Avalonia.Controls;
using Avalonia.Controls.Generators;

namespace Kaigara.Avalonia.Controls;
public class InspectorItem : TreeViewItem
{
    protected override IItemContainerGenerator CreateItemContainerGenerator()
        => CreateTreeItemContainerGenerator<InspectorItem>();
}
