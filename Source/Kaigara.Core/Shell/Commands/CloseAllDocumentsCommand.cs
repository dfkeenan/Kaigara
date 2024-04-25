using System.Reactive.Linq;
using Dock.Model.Core;
using Dock.Model.ReactiveUI.Controls;
using Kaigara.Commands;
using Kaigara.Menus;

namespace Kaigara.Shell.Commands;

[MenuItemDefinition("CloseAllDocuments", "/DocumentTabCommands")]
[CommandDefinition("Close All Tabs", IconName = "CloseDocumentGroup")]
public class CloseAllDocumentsCommand : ShellCommand
{
    protected override void OnExecute()
    {
        Shell.Documents.ToList().ForEach(document => 
        {
            if (document.Owner is not DocumentDock sourceDockableOwner) return;
            if (sourceDockableOwner.Factory is not IFactory factory) return;
            factory.CloseDockable(document);
        });
    }
}
