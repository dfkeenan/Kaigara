using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Primitives;

namespace Kaigara.Avalonia.Controls;
public class InspectorItemsControl : TreeView
{
    protected override ITreeItemContainerGenerator CreateTreeItemContainerGenerator()
        => CreateTreeItemContainerGenerator<InspectorItem>();
}
