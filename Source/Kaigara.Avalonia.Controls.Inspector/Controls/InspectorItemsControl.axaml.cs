using Avalonia.Controls;
using Avalonia.Controls.Generators;

namespace Kaigara.Avalonia.Controls;
public class InspectorItemsControl : TreeView
{
    protected override ITreeItemContainerGenerator CreateTreeItemContainerGenerator()
        => CreateTreeItemContainerGenerator<InspectorItem>();
}
