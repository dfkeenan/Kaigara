using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI.Controls;
using Kaigara.Shell.ViewModels;

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
