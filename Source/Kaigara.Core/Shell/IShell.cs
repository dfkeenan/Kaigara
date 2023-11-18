using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI.Controls;
using Kaigara.Services;

namespace Kaigara.Shell;

public interface IShell : IRequireStorageProvider
{
    IFactory Factory { get; }
    IRootDock Layout { get; }
    DockableCollection<Document> Documents { get; }
    DockableCollection<Tool> Tools { get; }
    ReadOnlyDockableCollection<IDockable> Dockables { get; }
}
