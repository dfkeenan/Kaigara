using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dock.Model.Controls;
using Dock.Model.Core;

namespace Kaigara.Shell
{
    public interface IShell
    {
        IFactory Factory { get; }
        IRootDock Layout { get; }

        IDocument? ActiveDocument { get; }
        ReadOnlyObservableCollection<IDocument> Documents { get; }
        IObservable<IDocument?> DocumentActivated { get; }

        ITool? ActiveTool { get; }
        ReadOnlyObservableCollection<ITool> Tools { get; }
        IObservable<ITool?> ToolActivated { get; }

        void OpenDocument(IDocument document);
        void OpenDocument<TDocument>() where TDocument : IDocument;
        void OpenTool(ITool tool, bool focus = false);
        void OpenTool<TTool>(bool focus = false) where TTool : ITool;
    }
}
