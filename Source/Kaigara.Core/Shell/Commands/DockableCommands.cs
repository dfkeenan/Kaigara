using System.Reactive.Linq;
using Dock.Model.Core;
using Kaigara.Commands;
using Kaigara.Menus;

namespace Kaigara.Shell.Commands;

public abstract class DockableCommandBase : RegisteredCommand<IDockable>
{
    public required IShell Shell { get; init; }

    protected internal override void OnRegistered(ICommandManager commandManager)
    {
        commandManager.RequerySuggested
                      .CombineLatest(Shell.Dockables.Active)
                      .Subscribe(e => NotifyCanExecuteChanged());
    }
}

[MenuItemDefinition("DocumentContextMenuFloatDockable", "DocumentContextMenu", DisplayOrder = 1, CanExecuteBehavior = CanExecuteBehavior.Visible)]
[CommandDefinition("Float", Name = "FloatDockable", IconName = "")]
public class FloatDockableCommand : DockableCommandBase
{
    protected override bool CanExecute(IDockable? parameter)
    {
        var target = parameter ?? Shell.Dockables.Active.Value;
        if (target == null) return false;

        return target.CanFloat;

    }

    protected override void OnExecute(IDockable? parameter)
    {
        var target = parameter ?? Shell.Dockables.Active.Value;
        if (target == null) return;

        target.Owner?.Factory?.FloatDockable(target);
    }
}
[MenuItemDefinition("DocumentContextMenuCloseDockable", "DocumentContextMenu", DisplayOrder = 2, CanExecuteBehavior = CanExecuteBehavior.Visible)]
[CommandDefinition("Close", Name = "CloseDockable", IconName = "")]
public class CloseDockableCommand : DockableCommandBase
{
    protected override bool CanExecute(IDockable? parameter)
    {
        var target = parameter ?? Shell.Dockables.Active.Value;
        if (target == null) return false;

        return target.CanClose;

    }

    protected override void OnExecute(IDockable? parameter)
    {
        var target = parameter ?? Shell.Dockables.Active.Value;
        if (target == null) return;

        target.Owner?.Factory?.CloseDockable(target);
    }
}


[MenuItemDefinition("DocumentContextMenuCloseOtherDockables", "DocumentContextMenu", DisplayOrder = 3, CanExecuteBehavior = CanExecuteBehavior.Visible)]
[CommandDefinition("Close Other Tabs", Name = "CloseOtherDockables", IconName = "CloseDocumentGroup")]
public class CloseOtherDockablesCommand : DockableCommandBase
{
    protected override bool CanExecute(IDockable? parameter)
    {
        var target = parameter ?? Shell.Dockables.Active.Value;
        if (target == null) return false;

        return target.CanClose;

    }

    protected override void OnExecute(IDockable? parameter)
    {
        var target = parameter ?? Shell.Dockables.Active.Value;
        if (target == null) return;

        target.Owner?.Factory?.CloseOtherDockables(target);
    }
}

[MenuItemDefinition("CloseAllDocuments", "/DocumentTabCommands")]
[MenuItemDefinition("DocumentContextMenuCloseAllDockables", "DocumentContextMenu", DisplayOrder = 4, CanExecuteBehavior = CanExecuteBehavior.Visible)]
[CommandDefinition("Close All Tabs", Name = "CloseAllDockables", IconName = "CloseDocumentGroup")]
public class CloseAllDockablesCommand : DockableCommandBase
{
    protected override bool CanExecute(IDockable? parameter)
    {
        var target = parameter ?? Shell.Dockables.Active.Value;
        if (target == null) return false;

        return target.CanClose;

    }

    protected override void OnExecute(IDockable? parameter)
    {
        var target = parameter ?? Shell.Dockables.Active.Value;
        if (target == null) return;

        target.Owner?.Factory?.CloseAllDockables(target);
    }
}
[MenuItemDefinition("DocumentContextMenuCloseLeftDockables", "DocumentContextMenu", DisplayOrder = 5, CanExecuteBehavior = CanExecuteBehavior.Visible)]
[CommandDefinition("Close Tabs To The Left", Name = "CloseLeftDockables", IconName = "CloseDocumentGroup")]
public class CloseLeftDockablesCommand : DockableCommandBase
{
    protected override bool CanExecute(IDockable? parameter)
    {
        var target = parameter ?? Shell.Dockables.Active.Value;
        if (target == null) return false;

        return target.CanClose;

    }

    protected override void OnExecute(IDockable? parameter)
    {
        var target = parameter ?? Shell.Dockables.Active.Value;
        if (target == null) return;

        target.Owner?.Factory?.CloseLeftDockables(target);
    }
}
[MenuItemDefinition("DocumentContextMenuCloseRightDockables", "DocumentContextMenu", DisplayOrder = 6, CanExecuteBehavior = CanExecuteBehavior.Visible)]
[CommandDefinition("Close Tabs To The Right", Name = "CloseRightDockables", IconName = "CloseDocumentGroup")]
public class CloseRightDockablesCommand : DockableCommandBase
{
    protected override bool CanExecute(IDockable? parameter)
    {
        var target = parameter ?? Shell.Dockables.Active.Value;
        if (target == null) return false;

        return target.CanClose;

    }

    protected override void OnExecute(IDockable? parameter)
    {
        var target = parameter ?? Shell.Dockables.Active.Value;
        if (target == null) return;

        target.Owner?.Factory?.CloseRightDockables(target);
    }
}
