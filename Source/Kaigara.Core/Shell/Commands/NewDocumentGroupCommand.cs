using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI.Controls;
using ReactiveUI;

namespace Kaigara.Shell.Commands;

public abstract class NewDocumentGroupCommand : ShellCommand
{
    protected override IObservable<bool> GetCanExecute()
    {
        return from doc in Shell.Documents.Active
               let dockables = doc?.Owner is DocumentDock dd ? dd.VisibleDockables as ObservableCollection<IDockable> : null
               from count in dockables switch
               {
                   { } => dockables.WhenAnyValue(dockables => dockables.Count),
                   _ => Observable.Return(0)
               }
               select count > 1;
    }
    protected void SplitDocumentGroup(Document sourceDockable, DockOperation operation)
    {
        
        if (sourceDockable.Owner is not DocumentDock sourceDockableOwner) return;
        if (sourceDockableOwner.Owner is not IDock targetDock) return;
        if (targetDock.Factory is not IFactory factory) return;

        var targetDocumentDock = factory.CreateDocumentDock();
        targetDocumentDock.Title = nameof(IDocumentDock);
        targetDocumentDock.VisibleDockables = factory.CreateList<IDockable>();
        if (sourceDockableOwner is IDocumentDock sourceDocumentDock)
        {
            targetDocumentDock.Id = sourceDocumentDock.Id;
            targetDocumentDock.CanCreateDocument = sourceDocumentDock.CanCreateDocument;

            if (sourceDocumentDock is IDocumentDockContent sourceDocumentDockContent
                && targetDocumentDock is IDocumentDockContent targetDocumentDockContent)
            {
                targetDocumentDockContent.DocumentTemplate = sourceDocumentDockContent.DocumentTemplate;
            }
        }
        factory.MoveDockable(sourceDockableOwner, targetDocumentDock, sourceDockable, null);
        factory.SplitToDock(targetDock, targetDocumentDock, operation);
    }
}
