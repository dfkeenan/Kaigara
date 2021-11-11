using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI.Controls;

namespace Kaigara.Shell
{
    public interface IShell
    {
        IFactory Factory { get; }
        IRootDock Layout { get; }
        DockableCollection<Document> Documents { get; }
        DockableCollection<Tool> Tools { get; }
        ReadOnlyDockableCollection<IDockable> Dockables { get; }
    }
}
