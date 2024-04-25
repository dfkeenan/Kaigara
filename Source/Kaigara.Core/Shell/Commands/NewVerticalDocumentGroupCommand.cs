using Dock.Model.Core;
using Dock.Model.ReactiveUI.Controls;
using Kaigara.Commands;
using Kaigara.Menus;

namespace Kaigara.Shell.Commands;

[MenuItemDefinition("NewVerticalDocumentGroup", "/DocumentTabCommands")]
[CommandDefinition("New Vertical Document Group", IconName = "SplitScreenVertically")]
public class NewVerticalDocumentGroupCommand : NewDocumentGroupCommand
{
    protected override void OnExecute()
    {
        if (Shell.Documents.Active.Value is not Document sourceDockable) return;
        SplitDocumentGroup(sourceDockable, DockOperation.Bottom);
    }
}