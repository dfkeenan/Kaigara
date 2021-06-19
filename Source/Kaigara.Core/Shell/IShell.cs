using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dock.Model.Controls;
using Dock.Model.Core;
using Kaigara.Shell.ViewModels;

namespace Kaigara.Shell
{
    public interface IShell
    {
        IFactory Factory { get; }
        IRootDock Layout { get; }
        DockableCollection<IDocument> Documents { get; }
        DockableCollection<ITool> Tools { get; }
    }
}
