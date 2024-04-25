using Dock.Model.Core;
using Dock.Model.ReactiveUI.Controls;
using Kaigara.Commands;
using Kaigara.Menus;

namespace Kaigara.Shell.Commands;

[MenuItemDefinition("NewHorizontalDocumentGroup", "/DocumentTabCommands")]
[CommandDefinition("New Horizontal Document Group", IconName = "SplitScreenHorizontally")]
public class NewHorizontalDocumentGroupCommand : NewDocumentGroupCommand
{
    protected override void OnExecute()
    {
        if (Shell.Documents.Active.Value is not Document sourceDockable) return;
        SplitDocumentGroup(sourceDockable, DockOperation.Right);
    }
}
